using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class Floor : BasicObject, ISoundOwner
{
    [SerializeField] private SoundHandler _soundHandler;

    public AudioClip GetSound() => _soundHandler.GetSound();
}
