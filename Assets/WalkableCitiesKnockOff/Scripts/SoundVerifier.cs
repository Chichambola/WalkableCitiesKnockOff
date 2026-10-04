using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

public class SoundVerifier : MonoBehaviour
{
    [SerializeField] private Vector2 _searchSize;
    [SerializeField] private ContactFilter2D _filter;
    [SerializeField] private int _collidersAmount = 50;
    
    private Collider2D[] _colliders;
    private Vector2 _point;
    private Vector2 _position;

    private void Awake()
    {
        _colliders = new Collider2D[_collidersAmount];
    }

    public ESoundType DetermineSound(Vector2 position, float angle)
    {
        _position = position;
        
        int hits = Physics2D.OverlapPoint(position, _filter, _colliders);

        Floor soundOwner = null;
        
        for (int i = 0; i < hits; i++)
        {
            var hit = _colliders[i];

            if (!hit.TryGetComponent(out Floor foundOwner))
                continue;
            
            if (soundOwner == null)
            {
                soundOwner = foundOwner;
            }
            else if (soundOwner.SortingLayerIndex < foundOwner.SortingLayerIndex)
            {
                soundOwner = foundOwner;
            }
        }
        
        return soundOwner?.Type ?? throw new Exception("There are no sound owners!");
    }
}
