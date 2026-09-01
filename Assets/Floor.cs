using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Floor : BasicObject, ISoundOwner
{
    [SerializeField] private List<AudioClip> _sounds;
    
    private Rigidbody2D _rigidbody;
    private Collider _collider;

    public List<AudioClip> Sounds => _sounds;
}
