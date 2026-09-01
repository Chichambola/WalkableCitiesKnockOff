using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class CollisionHandler : MonoBehaviour
{
    [SerializeField] private float _maxCollisionTime = 1f;
    [SerializeField] private float _timeMultiplier = 20f;
    
    public event Action HitWall;
    public event Action HitManyCollisons;
    public event Action<IKickable, Vector2> HitKickable;
    public event Action<ISoundOwner> HitSoundOwner;
    
    private float _collisionCount;
    private Rigidbody2D _rigidbody;
    private Collider2D _collider;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
    }

    private void OnValidate()
    {
        GetComponent<Rigidbody2D>().gravityScale = 0;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Wall wall))
        {
            if (wall is ISoundOwner soundOwner)
            {
                HitSoundOwner?.Invoke(wall);
            }
            
            HitWall?.Invoke();
        }
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.TryGetComponent(out IKickable kickable))
        {
            if (kickable is ISoundOwner soundOwner)
            {
                HitSoundOwner?.Invoke(soundOwner);
            }
            
            HitKickable?.Invoke(kickable, other.contacts[0].normal);
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        _collisionCount += Time.deltaTime * _timeMultiplier;

        if (_collisionCount > _maxCollisionTime)
        {
            _collider.enabled = false;
        }

        if (_collisionCount < _maxCollisionTime)
        {
            _collider.enabled = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        _collisionCount = 0;
    }

    private void FixedUpdate()
    {
        if (_collisionCount > _maxCollisionTime)
        {
            HitManyCollisons?.Invoke();
        }
    }
}
