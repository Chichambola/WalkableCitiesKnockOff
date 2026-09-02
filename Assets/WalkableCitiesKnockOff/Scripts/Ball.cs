using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : BasicObject, IKickable, ISoundOwner
{
    [SerializeField] private SoundHandler _soundHandler;
    
    public AudioClip GetSound() => _soundHandler.GetSound(); 

    public void ChangeDirection(Vector2 directionForce)
    {
        Rigidbody.AddForce(directionForce, ForceMode2D.Impulse);
    }
}
