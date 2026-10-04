using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
public class CollisionHandler : MonoBehaviour
{
    [SerializeField] private int _amountToCheck = 30;
    [SerializeField] private ContactFilter2D _filter;
    [SerializeField] private MovementVerifier _verifier;
    
    public event Action HitWall;
    public event Action<IKickable, Vector2> HitKickable;
    public event Action<ISoundOwner> HitSoundOwner;
    
    private Collider2D _collider;
    private Rigidbody2D _rigidbody;
    private Vector2 _direction;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        _verifier.DetectedCollider += Process;
    }

    private void OnDisable()
    {
        _verifier.DetectedCollider -= Process;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        var otherCollider = other.collider;
        
        _direction = other.contacts[0].point;
        
        Process(otherCollider);
    }

    private void Process(Collider2D other)
    {
        if (other.TryGetComponent(out Wall _))
        {
            HitWall?.Invoke();
        }

        if (other.TryGetComponent(out ISoundOwner soundOwner) && !other.TryGetComponent(out IKickable _))
        {
            HitSoundOwner?.Invoke(soundOwner);
        }
        
        if (other.TryGetComponent(out IKickable kickable))
        {
            HitKickable?.Invoke(kickable, _direction);
        }
    }
}
