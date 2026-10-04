using System;
using System.Collections;
using System.Collections.Generic;
using AYellowpaper;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

public class ScoreHandler : Singleton<ScoreHandler>
{
    [SerializeField] private SceneReadyChannel _channel;
    [SerializeField] private TextMeshProUGUI _text;
    
    private IPlayer _player;
    
    private static int s_currentStepCount;
    private static int s_wholeAmount;
    private static TextMeshProUGUI s_text;
    
    private void OnEnable()
    {
        Game.RequestedRestart += Restore;
        Game.RequestedNextLevel += OnRequestedNextLevel;
        _channel.Raised += OnLoaded;
        
        _text.enabled = true;
        s_text = _text;
        
        UpdateText();
    }

    private void OnDisable()
    {
        Game.RequestedRestart -= Restore;
        Game.RequestedNextLevel -= OnRequestedNextLevel;
        _channel.Raised -= OnLoaded;

        s_currentStepCount = 0;
        s_wholeAmount = 0;
    }
    
    public static void Hide()
    {
        s_text.enabled = false;
    }
    
    private void OnLoaded(IPlayer player)
    {
        if (_player != null)
            _player.Stepped -= OnStep;
        
        _player = player;

        _player.Stepped += OnStep;
    }

    private async UniTask OnRequestedNextLevel()
    {
        Restore();

        await UniTask.Yield();
    }

    private void Restore()
    {
        s_wholeAmount -= s_currentStepCount;
        
        s_currentStepCount = 0;

        if (!s_text.isActiveAndEnabled)
            s_text.enabled = true;
        
        UpdateText();
    }
    
    private void OnStep()
    {
        s_wholeAmount++;
        
        s_currentStepCount++;
        
        UpdateText();
    }

    private void UpdateText()
    {
        _text.text = $"Score: {s_currentStepCount}";
    }
}
