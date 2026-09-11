using System;
using System.Collections;
using System.Collections.Generic;
using AYellowpaper;
using TMPro;
using UnityEngine;

public class FirstText : MonoBehaviour
{
    [SerializeField] private SceneHandler _sceneHandler;
    [SerializeField] private TextMeshProUGUI _text;

    private IPlayer _player;

    private void OnEnable()
    {
        _sceneHandler.Loaded += OnSceneHandlerLoaded;
    }

    private void OnDisable()
    {
        _sceneHandler.Loaded -= OnSceneHandlerLoaded;
    }

    private void Start()
    {
        _text.text = $"Press {_player.ChangeFootButton.ToUpper()} to start.";
    }
    
    private void OnSceneHandlerLoaded(IPlayer player)
    {
        _player = player;
    }
}
