using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using PrimeTween;
using UnityEngine;

public class Dog : BasicObject
{
    [SerializeField] private SearchableFinder _birdSearcher;
    [SerializeField] private Kicker _kicker;
    [SerializeField] private CollisionHandler _collisionHandler;
    [Header("Tween settings")]
    [SerializeField] private float _interval = 2f;
    [SerializeField] private float _force = 1.5f;

    private CancellationTokenSource _cts;

    private void OnEnable()
    {
        _collisionHandler.HitKickable += OnHitKickable;
        
        _cts = new CancellationTokenSource();
        
        LookForTask(_cts.Token).Forget();
    }

    private void OnDisable()
    {
        _collisionHandler.HitKickable -= OnHitKickable;
        
        _cts?.Dispose();
    }
    
    private async UniTaskVoid LookForTask(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_interval), cancellationToken: token);
            
            if (!_birdSearcher.TryFind(out var hits))
                continue;

            if (hits.Count == 0)
                continue;
            
            bool isBlocked = true;

            ISearchable searchable = null;
            
            while (isBlocked && hits.Count > 0)
            {
                searchable = _birdSearcher.FindClosest(hits);

                if (_birdSearcher.IsBlocked(searchable))
                {
                    if (hits.Count == 0)
                        continue;
                    
                    hits.Remove(searchable);

                    searchable = null;
                }
                else
                    isBlocked = false;
            }

            if (searchable == null)
                continue;

            Vector2 direction = transform.position - new Vector3(searchable.Position.x, searchable.Position.y);

            Rigidbody.velocity = -direction.normalized * _force;
            
            await UniTask.Delay(TimeSpan.FromSeconds(_interval), cancellationToken: token);
        }
    }
    
    private void OnHitKickable(IKickable kickable, Vector2 direction) => _kicker.Execute(kickable, direction);
}