using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StuckPreventor : MonoBehaviour
{
    [SerializeField] private BoxCollider2D[] _collidersTocheck;
    [SerializeField] private ContactFilter2D _filter;
    [SerializeField] private int _amountToCheck = 50;
    [SerializeField] private float _distance = 5;

    public event Action<Vector2> DetectedOverlap;
    
    private Collider2D[] _colliders;
    
    private void Awake()
    {
        _colliders = new Collider2D[_amountToCheck];
    }

    private void FixedUpdate()
    {
        foreach (var collider in _collidersTocheck)
        {
            var position = collider.transform.position;

            var size = collider.size;

            var angle = collider.transform.rotation.eulerAngles.z;
            
           var hits =  Physics2D.OverlapBox(position, size, angle, _filter, _colliders);
           
            for (int i = 0; i < hits; i++)
            {
                var hitCollider = _colliders[i];

                ColliderDistance2D dist = Physics2D.Distance(collider, hitCollider);

                if (dist.isOverlapped)
                {
                    Vector2 separation = dist.normal * dist.distance;
                    
                    DetectedOverlap?.Invoke(separation);
                }
            }
        }
    }
}
