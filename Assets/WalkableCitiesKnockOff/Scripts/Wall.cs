using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : BasicObject, ISoundOwner
{
    [SerializeField] private SoundHandler _soundHandler;
    public AudioClip GetSound() => _soundHandler.GetSound();
}
