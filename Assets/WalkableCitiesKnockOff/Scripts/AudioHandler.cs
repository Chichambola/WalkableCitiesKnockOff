using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using AYellowpaper;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioHandler : Singleton<AudioHandler>
{
    [SerializeField] private float _volume;
    [SerializeField] private SceneReadyChannel _channel;
    [SerializeField] private SoundRequestChannel _soundRequest;
    [SerializeField] private SerializedDictionary<ESoundType, List<AudioClip>> _soundClips;

    private IPlayer _player;

    protected override void Awake()
    {
        base.Awake();

        _channel.Raised += OnGameLoaded;
        _soundRequest.Raised += PlaySound;
    }

    private void OnGameLoaded(IPlayer player)
    {
        if (_player != null)
        {
            _player.HitSoundOwner -= PlaySound;

            _player = null;
        }
        
        _player = player; 
        
        _player.HitSoundOwner += PlaySound;
    }
    
    private void OnDisable()
    {
        _channel.Raised -= OnGameLoaded;
        _soundRequest.Raised -= PlaySound;
        _player.HitSoundOwner -= PlaySound;
    }

    private void PlaySound(ESoundType soundType, Vector2 position)
    {
        var sounds = _soundClips[soundType];

        if (_soundClips.Count <= 0)
            throw new Exception($"You didnt assign any sounds to this {soundType} type");
        
        var randomIndex = Random.Range(0, sounds.Count);

        var sound = sounds[randomIndex];
        
        AudioSource.PlayClipAtPoint(sound, position, _volume);
    }
}

public enum ESoundType
{
    Wall,
    Grass,
    Water,
    Dirt,
    Ball,
    Whistle,
    Bird
}
