using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : BasicObject, ISoundOwner
{
    [SerializeField] private List<AudioClip> _sounds;

    public List<AudioClip> Sounds => _sounds;
}
