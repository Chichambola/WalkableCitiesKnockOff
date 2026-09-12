using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D), typeof(Rigidbody2D))]
public class EndOfLevel : BasicObject
{
    public event Action<IPlayer> PlayerDetected;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out IPlayer player))
            PlayerDetected?.Invoke(player);
    }
}
