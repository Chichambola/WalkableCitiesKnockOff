using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioHandler : MonoBehaviour
{
    [SerializeField] private InterfaceReference<IPlayer, MonoBehaviour> _playerPrefab;
    [SerializeField] private float _volume;

    private IPlayer _player;
    
    private void Awake()
    {
        _player = _playerPrefab.Value;
    }

    private void OnEnable()
    {
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
