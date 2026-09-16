using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Collider2D))]
public class CollisionHandler : MonoBehaviour
{
    [SerializeField] private float _maxCollisionFrames = 50f;
    
    public event Action HitWall;
    public event Action HitManyCollisons;
    public event Action LeftCollider;
    public event Action<IKickable, Vector2> HitKickable;
    public event Action<ISoundOwner> HitSoundOwner;
    
    private float _collisionCount;
    private Collider2D _collider;
    private float _triggerCount;
    private int _startFrame;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
    }

    private void Start()
    {
        _startFrame = Time.frameCount;
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.TryGetComponent(out Wall wall))
        {
            HitWall?.Invoke();
            
            if (wall is ISoundOwner soundOwner)
            {
                HitSoundOwner?.Invoke(soundOwner);
            }
        }
        
        if (other.collider.TryGetComponent(out IKickable kickable))
        {
            HitKickable?.Invoke(kickable, other.contacts[0].normal);
            
            if (kickable is ISoundOwner soundOwner)
            {
                HitSoundOwner?.Invoke(soundOwner);
            }
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        CalculateFrames();
    } 
    
    private void CalculateFrames()
    {
        int framesPassed = Time.frameCount - _startFrame;

        if (framesPassed >= _maxCollisionFrames)
        {
            _startFrame = Time.frameCount;
            _collider.enabled = false;
            HitManyCollisons?.Invoke();
        }
    }
}
