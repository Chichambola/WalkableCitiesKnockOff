using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayer
{
    public event Action<ISoundOwner> HitSoundOwner;
    public event Action RequestedRestart;
    public event Action Stepped;
    public Vector2 CurrentPosition { get; }
    public bool IsAccepting { get; }
    public string AcceptButton { get; }
    public string RestartButton { get; }
    public string ChangeFootButton { get; }
}
