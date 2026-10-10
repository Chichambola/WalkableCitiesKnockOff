using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Searcher;
using UnityEngine;

public class Searcher<T> : MonoBehaviour where T : ISearchable
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

        Debug.Log(detectedHits.Count);
        
        return detectedHits.Count != 0;
    }

    public T FindClosest(List<T> objects)
    {
        if (objects.Count == 0)
            throw new Exception("List count is 0");
        
        T closest = default(T);

        float minDistance = Mathf.Infinity;
        
        Vector2 currentPos = transform.position;

        foreach (var T in objects)
        {
            float distance = Vector2.Distance(T.Position, currentPos);

            if (distance < minDistance)
            {
                minDistance = distance;
                closest = T;
            }
        }

        return closest;
    }
    
    public bool IsBlocked(T @object)
    {
        var hits = Physics2D.Linecast(transform.position, @object.Position, _filter, _hits);

        for (int i = 0; i < hits; i++)
        {
            if (_hits[i].collider.TryGetComponent(out IBlockable _))
            {
                return true;
            }
        }

        return false;
    }
}