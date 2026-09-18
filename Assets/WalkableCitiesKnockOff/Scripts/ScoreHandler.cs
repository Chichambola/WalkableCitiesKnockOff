using System;
using System.Collections;
using System.Collections.Generic;
using AYellowpaper;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

public class ScoreHandler : Singleton<ScoreHandler>
{
    [SerializeField] private TextMeshProUGUI _text;
    
    private IPlayer _player;
    
    private static int s_currentStepCount;
    private static int s_wholeAmount;
    
    private void OnEnable()
    {
        Game.RequestedRestart += Restore;
        Game.RequestedNextLevel += OnRequestedNextLevel;
        Game.Loaded += OnLoaded;
        
        UpdateText();
    }

    private void OnDisable()
    {
        Game.RequestedRestart -= Restore;
        Game.RequestedNextLevel -= OnRequestedNextLevel;
        Game.Loaded -= OnLoaded;

        s_currentStepCount = 0;
        s_wholeAmount = 0;
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
