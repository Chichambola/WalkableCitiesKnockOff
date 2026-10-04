using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class Guardian : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private SceneReadyChannel _channel;
    [SerializeField] private SoundRequestChannel _soundRequest;
    [SerializeField] private ESoundType _typeToProtect = ESoundType.Grass;
    [SerializeField] private ESoundType _soundType = ESoundType.Whistle;
    [SerializeField] private Vector2 _offset;

    private IPlayer _player;

    private void OnEnable()
    {
        _channel.Raised += OnLoaded;

        _canvas.enabled = false;
    }
    
    private void OnDisable()
    {
        _channel.Raised -= OnLoaded;
        
        if (_player != null)
            _player.HitSoundOwner -= OnPlayerHit;
    }

    private void OnLoaded(IPlayer player)
    {
        if (_player != null)
        {
            _player.HitSoundOwner -= OnPlayerHit;

            _player = null;
        }
        
        _player = player ?? throw new Exception("Player is null!");
        
        _player.HitSoundOwner += OnPlayerHit;
    }

    private void OnPlayerHit(ESoundType soundType, Vector2 position)
    {
        if (soundType != _typeToProtect)
            return;

        Vector3 newPos = new Vector2(_player.Position.x + _offset.x, _player.Position.y + _offset.y);
        
        transform.position = newPos;
        
        _soundRequest.Raise(_soundType, position);
        
        Game.Pause();
        
        _canvas.enabled = true;

        ScoreHandler.Hide();
    }
}