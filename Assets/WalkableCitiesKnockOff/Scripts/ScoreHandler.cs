using System;
using System.Collections;
using System.Collections.Generic;
using AYellowpaper;
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
        _player = Game.GetPlayer();
        _player.Stepped += OnStep;
        Game.RequestedRestart += Restore;
        
        UpdateText();
    }

    private void OnDisable()
    {
        _player.Stepped -= OnStep;
        Game.RequestedRestart -= Restore;

        s_currentStepCount = 0;
        s_wholeAmount = 0;
    }

    public void Restore()
    {
        _player.Stepped -= OnStep;

        _player = Game.GetPlayer();
        
        _player.Stepped += OnStep;

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
