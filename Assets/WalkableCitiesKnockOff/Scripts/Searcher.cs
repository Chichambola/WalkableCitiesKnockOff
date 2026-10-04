using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Searcher<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] private float _radius = 10f;
    [SerializeField] private ContactFilter2D _filter;
    [SerializeField] private int _collidersToCheck = 50;

    private RaycastHit2D[] _hits;
    
    private void OnEnable()
    {
        _hits = new RaycastHit2D[_collidersToCheck];
    }

    public bool TryFind(out List<T> detectedHits)
    {
        var hits = Physics2D.CircleCast(transform.position, _radius, Vector2.zero, _filter, _hits);

        detectedHits = new();

        for (int i = 0; i < hits; i++)
        {
            if (_hits[i].collider.TryGetComponent(out T type))
            {
                detectedHits.Add(type);
            }
        }

        return detectedHits.Count != 0;
    }

    public T FindClosest(List<T> objects)
    {
        if (objects.Count == 0)
            throw new Exception("List count is 0");
        
        T closest = null;

        float minDistance = Mathf.Infinity;
        
        Vector2 currentPos = transform.position;

        foreach (var T in objects)
        {
            float distance = Vector2.Distance(T.transform.position, currentPos);

            if (distance < minDistance)
            {
                minDistance = distance;
                closest = T;
            }
        }

        return closest;
    }
}