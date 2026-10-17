using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEditor.Profiling;
using UnityEngine;
public class MovementVerifier : MonoBehaviour
{
    [SerializeField] private int _amountToCheck = 30;
    [SerializeField] private ContactFilter2D _filter;

    public event Action<Collider2D> DetectedCollider;
    
    private RaycastHit2D[] _hits;

    private void Awake()
    {
        _hits = new RaycastHit2D[_amountToCheck];
    }
    
    public bool TryMove(Rigidbody2D body, Vector2 nextPosition, out Wall wall)
    {
        Vector2 moveDelta = nextPosition - body.position;

        int hitCount = body.Cast(moveDelta, _filter, _hits, moveDelta.magnitude);

        if (hitCount <= 0)
        {
            wall = null;
            
            return true;
        }
        
        var hit = _hits.First();

        var hitCollider = hit.collider;

        wall = hitCollider.TryGetComponent(out Wall foundWall) ? foundWall : null;
        
        DetectedCollider?.Invoke(hitCollider);

        return false;
    }
}
