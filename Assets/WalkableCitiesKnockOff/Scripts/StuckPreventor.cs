using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class StuckPreventor : MonoBehaviour
{
    [SerializeField] private BoxCollider2D[] _collidersTocheck;
    [SerializeField] private ContactFilter2D _filter;
    [SerializeField] private int _amountToCheck = 50;
    [SerializeField] private float _distance = 5f;

    public event Action<Vector2> DetectedOverlap;
    
    private Collider2D[] _colliders;
    private CancellationTokenSource _cts;
    private Collider2D _hitCollider;
    private Vector2 _offset;
    private Vector3 _currentColliderPos;
    
    private void Awake()
    {
        _colliders = new Collider2D[_amountToCheck];
    }

    private void OnEnable()
    {
        _cts = new CancellationTokenSource();
        
        PreventStuckTask(_cts.Token).Forget();
    }

    private void OnDisable()
    {
        _cts?.Cancel();
    }

    private void OnDestroy()
    {
        _cts?.Dispose();
    }

    private async UniTask PreventStuckTask(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            int stuckCount = 0;
            
            foreach (var collider in _collidersTocheck)
            {
                _currentColliderPos = collider.transform.position;

                var size = collider.size;

                var angle = collider.transform.rotation.eulerAngles.z;
            
                var hits =  Physics2D.OverlapBox(_currentColliderPos, size, angle, _filter, _colliders);

                if (hits == 0)
                    continue;
                
                for (int i = 0; i < hits; i++)
                {
                    _hitCollider = _colliders[i];

                    ColliderDistance2D dist = Physics2D.Distance(collider, _hitCollider);

                    if (dist.isOverlapped)
                    {
                        stuckCount++;
                        
                        Vector2 separation = dist.normal * dist.distance;
                        
                        DetectedOverlap?.Invoke(separation);
                    }
                    
                    await UniTask.DelayFrame(5, PlayerLoopTiming.FixedUpdate, cancellationToken: token);
                }
            }
            
            await UniTask.WaitForFixedUpdate(cancellationToken: token);
        }
    }
}
