using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Floor : BasicObject, ISoundOwner
{
    [SerializeField] private SoundHandler _soundHandler;
    
    private Rigidbody2D _rigidbody;
    private Collider _collider;

    public AudioClip GetSound() => _soundHandler.GetSound();
}
