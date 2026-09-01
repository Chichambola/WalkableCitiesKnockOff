using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISoundOwner
{
    public List<AudioClip> Sounds { get; }
}
