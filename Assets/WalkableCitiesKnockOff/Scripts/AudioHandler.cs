using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioHandler : Singleton<AudioHandler>
{
    [SerializeField] private float _volume;

    private IPlayer _player;
    
    public void Init(IPlayer player)
    {
        _player = player;
        
        _player.HitSoundOwner += PlaySound;
    }

    private void OnDisable()
    {
        _player.HitSoundOwner -= PlaySound;
    }

    private void PlaySound(ISoundOwner soundOwner)
    {
        var sound = soundOwner.GetSound();
        
        AudioSource.PlayClipAtPoint(sound, _player.CurrentPosition, _volume);
    }
}
