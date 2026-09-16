using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Collider2D))]
public class CollisionHandler : MonoBehaviour
{
    [SerializeField] private float _maxCollisionFrames = 50f;
    
    public event Action HitWall;
    public event Action Stuck;
    public event Action<IKickable, Vector2> HitKickable;
    public event Action<ISoundOwner> HitSoundOwner;
    
    private Collider2D _collider;
    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
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

    private async void OnCollisionStay(Collision other)
    {
        await UniTask.Delay(TimeSpan.FromTicks(10));

        Stuck?.Invoke();
    }
}
