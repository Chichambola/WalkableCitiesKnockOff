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
    private List<AudioClip> _sounds;
    
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
        _sounds = soundOwner.Sounds.ToList();
        
        int randomIndex = Random.Range(0, _sounds.Count);
            
        AudioSource.PlayClipAtPoint(_sounds[randomIndex], _player.CurrentPosition, _volume);
    }
}
