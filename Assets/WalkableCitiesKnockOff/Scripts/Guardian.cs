using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(SpriteRenderer))]
public class Guardian : MonoBehaviour
{
    [SerializeField] private Canvas _textCanvas;
    [SerializeField] private SceneReadyChannel _channel;
    [SerializeField] private SoundRequestChannel _soundRequest;
    [SerializeField] private Vector2 _offset;
    [Header("Sound types")]
    [SerializeField] private ESoundType _typeToProtect = ESoundType.Grass;
    [SerializeField] private ESoundType _soundType = ESoundType.Whistle;
    [Header("States Sprites")]
    [SerializeField] private Sprite _idleSprite;
    [SerializeField] private Sprite _stopSprite;
    [SerializeField] private Canvas _speechBubble;

    private IPlayer _player;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = _idleSprite;
    }

    private void OnEnable()
    {
        _channel.Raised += OnLoaded;

        _textCanvas.gameObject.SetActive(false);
        _speechBubble.gameObject.SetActive(false);
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

        _spriteRenderer.sprite = _stopSprite;
        
        transform.position = newPos;
        
        _soundRequest.Raise(_soundType, position);
        
        Game.Pause();
        
        _textCanvas.gameObject.SetActive(true);
        _speechBubble.gameObject.SetActive(true);

        ScoreHandler.Hide();
    }
}