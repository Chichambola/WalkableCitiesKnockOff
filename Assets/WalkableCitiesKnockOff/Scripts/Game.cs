using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using PrimeTween;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Profiling;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class Game : Singleton<Game>
{
    [SerializeField] private Player _playerPrefab;
    [SerializeField] private AudioHandler _audioHandler;
    [SerializeField] private SceneReadyChannel _channel;

    public static event Action RequestedRestart;
    public static event Func<UniTask> RequestedNextLevel;
    
    private static float s_normalTime = 1f;
    private static float s_slowTime = 0.000001f;
    private int _tweenCapacity = 3000;
    private Player _player;
    private Vector2 _startPosition = new (9999, 9999);

    public static bool IsPaused => Mathf.Approximately(Time.timeScale, s_slowTime);

    private void Start()
    {
        PrimeTweenConfig.SetTweensCapacity(_tweenCapacity);
    }

    private void OnEnable()
    {
        Time.timeScale = s_normalTime;
        
        CreatePlayer();
        
        EndOfLevelHandler.RequestedEndOfLevel += LoadNextLevel;
        EndOfLevelHandler.DetectedPlayer += OnPlayerDetected;
        SceneLoader.Loaded += OnAllScenesLoaded;
    }
    
    private void OnDisable()
    {
        _player.RequestedRestart -= Restart;
        EndOfLevelHandler.RequestedEndOfLevel -= LoadNextLevel;
        SceneLoader.Loaded -= OnAllScenesLoaded;
    }
    
    public static void Pause()
    {
        Time.timeScale = s_slowTime;
    }

    public static void Resume()
    {
        Time.timeScale = s_normalTime;
    }
    
    private void LoadNextLevel()
    {
        _player.Set(false);
        
        ProcessPlayer();
        
        RequestedNextLevel?.Invoke();
    }

    private void Restart() => RequestedRestart?.Invoke();
    
    private void ProcessPlayer()
    {
        Resume();
        
        _player.Set(_startPosition);
        
        _player.ResetCharacteristics();
        
        _channel.Raise(_player);
    }
    
    private void CreatePlayer()
    {
        if (_player != null)
            return;
        
        _player = Instantiate(_playerPrefab, _startPosition, quaternion.identity);

        _player.transform.parent = transform;

        _player.RequestedRestart += Restart;
    }
    
    private void OnAllScenesLoaded()
    {
        ProcessPlayer();
        
        _player.Set(true);
        
        _channel.Raise(_player);
    }
    
    private void OnPlayerDetected() => _player.Set(false);
}
