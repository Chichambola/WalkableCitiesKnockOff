using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : BasicObject, IKickable, ISoundOwner
{
    [SerializeField] private List<AudioClip> _sounds;

    public List<AudioClip> Sounds => _sounds;

    public void ChangeDirection(Vector2 directionForce)
    {
        Rigidbody.AddForce(directionForce, ForceMode2D.Impulse);
    }
}
