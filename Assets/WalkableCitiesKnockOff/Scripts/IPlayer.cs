using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayer
{
    public event Action<ISoundOwner> HitSoundOwner; 
    public Vector2 CurrentPosition { get; }
}
