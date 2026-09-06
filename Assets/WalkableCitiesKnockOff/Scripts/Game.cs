using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class Game : MonoBehaviour
{
    [SerializeField] private Player _playerPrefab;
    [SerializeField] private TextMeshProUGUI _firstText;
    [SerializeField] private EndOfLevelHandler _levelHandler;
    [SerializeField] private SceneHandler _sceneHandler;

    private static float _normalTime = 1f;
    private static float _slowTime = 0.000001f;
    private static Player _player;
    private static Game Instance;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
    }

    private void OnEnable()
    {
        Time.timeScale = _normalTime;
        
        _player = _playerPrefab;
        _player.RequestedRestart += Restart;
        _player.PressedKey += OnPressedKey;
        
        _levelHandler.Init(_player);
    }

    private void OnDisable()
    {
        _player.RequestedRestart -= Restart;

        if (!_player.HasPressedKey)
            _player.PressedKey -= OnPressedKey;
    }

    private void Start()
    {
        _firstText.text = $"Press {_player.ChangeFootButton.ToUpper()} to start.";
    }

    public static void Pause()
    {
        Time.timeScale = _slowTime;
        
        _player.Freeze();
    }

    public static void Resume()
    {
        Time.timeScale = _normalTime;
        
        _player.UnFreeze();
    }
    
    private void OnPressedKey()
    {
        _firstText.text = "";

        _playerPrefab.PressedKey -= OnPressedKey;
    }
    
    private void Restart()
    {
        _sceneHandler.Restart();
    }
}
