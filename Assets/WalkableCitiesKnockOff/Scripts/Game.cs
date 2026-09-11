using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using PrimeTween;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class Game : Singleton<Game>
{
    [SerializeField] private Player _playerPrefab;
    [SerializeField] private ScoreHandler _scoreHandler;
    [SerializeField] private AudioHandler _audioHandler;

    public static event Action RequestedRestart;
    public static event Func<UniTask> RequestedNextLevel;
    
    private static float s_normalTime = 1f;
    private static float s_slowTime = 0.000001f;
    private static Player s_player;
    private int _tweenCapacity = 3000;

    private void Start()
    {
        PrimeTweenConfig.SetTweensCapacity(_tweenCapacity);
    }

    private void OnEnable()
    {
        Time.timeScale = s_normalTime;
        
        CreatePlayer();

        InitializeServices();
    }

    private void OnDisable()
    {
        s_player.RequestedRestart -= Restart;
    }
    
    public static void Pause()
    {
        Time.timeScale = s_slowTime;
        
        s_player.Freeze();
    }

    public static void Resume()
    {
        Time.timeScale = s_normalTime;
        
        s_player.UnFreeze();
    }

    public static Player GetPlayer() => s_player;
    
    public static void LoadNextLevel()
    {
        Resume();
        
        RequestedNextLevel?.Invoke();
    }
    
    private void Restart()
    {
        _scoreHandler.Restore();
        
        s_player.RequestedRestart -= Restart;
        
        Destroy(s_player.gameObject);
        
        RequestedRestart?.Invoke();
        
        Resume();
        
        CreatePlayer();
        
        InitializeServices();
    }
    
    private void CreatePlayer()
    {
        s_player = Instantiate(_playerPrefab, new Vector3(0,0,0), quaternion.identity);

        s_player.transform.parent = transform;

        s_player.RequestedRestart += Restart;
    }
    
    private void InitializeServices()
    {
        _scoreHandler.Init(s_player);
        _audioHandler.Init(s_player);
    }
}
