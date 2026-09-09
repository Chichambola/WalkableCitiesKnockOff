using System;
using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class Game : MonoBehaviour
{
    [SerializeField] private Player _playerPrefab;
    [SerializeField] private SceneHandler _sceneHandler;
    [SerializeField] private TextMeshProUGUI _scoreText;

    private static float s_normalTime = 1f;
    private static float s_slowTime = 0.000001f;
    private static Player s_player;
    private static Game s_Instance;
    private int _tweenCapacity = 3000;
    
    private void Awake()
    {
        if (s_Instance != null && s_Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        s_Instance = this;
    }

    private void Start()
    {
        _scoreText.SetText($"Score: {StepCounter.Score}");
        
        PrimeTweenConfig.SetTweensCapacity(_tweenCapacity);
    }

    private void OnEnable()
    {
        Time.timeScale = s_normalTime;
        
        s_player = _playerPrefab;
        s_player.RequestedRestart += Restart;
        s_player.Stepped += OnPlayerStepped;
    }

    private void OnDisable()
    {
        s_player.RequestedRestart -= Restart;
        s_player.Stepped -= OnPlayerStepped;
        
        StepCounter.Reset();
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
    
    private void Restart()
    {
        StepCounter.Reset();
        
        _sceneHandler.Restart();
    }
    
    private void OnPlayerStepped()
    {
        StepCounter.Increment();
        
        _scoreText.text = $"Score: {StepCounter.Score}";
    }
}
