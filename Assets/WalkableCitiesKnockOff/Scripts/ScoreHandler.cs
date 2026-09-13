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
    
    private int _currentStepCount;
    private int _wholeAmount;
    
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
        
        Restore();
    }

    public void Restore()
    {
        _player.Stepped -= OnStep;

        _player = Game.GetPlayer();
        
        _player.Stepped += OnStep;
        
        _currentStepCount = 0;
        
        UpdateText();
    }

    public void CalculateTotalScore()
    {
        _wholeAmount += _currentStepCount;
    }
    
    private void OnStep()
    {
        _currentStepCount++;
        
        UpdateText();
    }

    private void UpdateText()
    {
        _text.text = $"Score: {_currentStepCount}";
    }
}
