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

    public static event Action RequestedRestart;
    public static event Action<IPlayer> Loaded;
    public static event Func<UniTask> RequestedNextLevel;
    
    private static float s_normalTime = 1f;
    private static float s_slowTime = 0.000001f;
    private static Player s_player;
    private int _tweenCapacity = 3000;
    private Vector3 _startPosition = new Vector3(999, 999, 999);

    private void Start()
    {
        PrimeTweenConfig.SetTweensCapacity(_tweenCapacity);
    }

    private void OnEnable()
    {
        Time.timeScale = s_normalTime;
        
        PerformLevelStart();
        
        EndOfLevelHandler.RequestedEndOfLevel += LoadNextLevel;
        SceneLoader.Loaded += OnAllScenesLoaded;
    }

    private void OnDisable()
    {
        s_player.RequestedRestart -= Restart;
        EndOfLevelHandler.RequestedEndOfLevel -= LoadNextLevel;
        SceneLoader.Loaded -= OnAllScenesLoaded;
    }
    
    public static void Pause()
    {
        Time.timeScale = s_slowTime;
        
        s_player.Freeze();
    }

    public static void Resume()
    {
        Time.timeScale = s_normalTime;

        if (s_player != null)
            s_player.UnFreeze();
    }
    
    private void LoadNextLevel()
    {
        RequestedNextLevel?.Invoke();
        
        PerformLevelStart();
        
        s_player.Freeze();
        
        Loaded?.Invoke(s_player);
    }

    private void Restart()
    {
        RequestedRestart?.Invoke();
        
        PerformLevelStart();
        
        s_player.UnFreeze();
        
        Loaded?.Invoke(s_player);
    }
    
    private void CreatePlayer()
    {
        if (s_player != null)
        {
            s_player.RequestedRestart -= Restart;
        
            Destroy(s_player.gameObject);
        }
        
        s_player = Instantiate(_playerPrefab, _startPosition, quaternion.identity);
        
        s_player.Freeze();

        s_player.transform.parent = transform;

        s_player.RequestedRestart += Restart;
    }
    
    private void InitializeServices()
    {
        _audioHandler.Init(s_player);
    }
    
    private void PerformLevelStart()
    {
        Resume();
        
        CreatePlayer();
        
        InitializeServices();
    }
    
    private void OnAllScenesLoaded()
    {
        s_player.UnFreeze();
        
        Loaded?.Invoke(s_player);
    }
}
