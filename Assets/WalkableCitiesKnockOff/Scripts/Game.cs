using System;
using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class Game : MonoBehaviour
{
    [SerializeField] private Player _playerPrefab;
    [SerializeField] private ScoreHandler _scoreHandler;

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
        PrimeTweenConfig.SetTweensCapacity(_tweenCapacity);
    }

    private void OnEnable()
    {
        Time.timeScale = s_normalTime;
        
        s_player = _playerPrefab;
        s_player.RequestedRestart += Restart;
        
        _scoreHandler.Init(s_player);
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
    
    private void Restart()
    {
        _scoreHandler.Restore();
    }
}
