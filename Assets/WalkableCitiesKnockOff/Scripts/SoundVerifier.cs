using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundVerifier : MonoBehaviour
{
    [SerializeField] private Vector2 _searchSize;
    [SerializeField] private ContactFilter2D _filter;
    
    private List<RaycastHit2D> _colliders;

    private void Awake()
    {
        _colliders = new List<RaycastHit2D>();
    }

    public ISoundOwner DetermineSound(Vector2 position)
    {
        int hits = Physics2D.BoxCast(position, _searchSize, 0f, Vector2.up,_filter, _colliders);

        for (int i = 0; i < hits; i++)
        {
            if (_colliders[i].collider.TryGetComponent(out ISoundOwner soundOwner))
            {
                return soundOwner;
            }
        }

        return null;
    }
}
