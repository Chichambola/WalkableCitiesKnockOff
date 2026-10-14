using System;
using System.Collections;
using System.Collections.Generic;
using AYellowpaper;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ScoreHandler : Singleton<ScoreHandler>
{
    [SerializeField] private SceneReadyChannel _channel;
    [SerializeField] private Canvas _scoreCanvas;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private Canvas _totalScore;
    [SerializeField] private TextMeshProUGUI _totalScoreText;
    
    private IPlayer _player;
    
    private static int s_currentStepCount;
    private static int s_wholeStepAmount;
    private static Canvas s_scoreCanvas;
    
    private void OnEnable()
    {
        Game.RequestedRestart += Restore;
        Game.RequestedNextLevel += OnRequestedNextLevel;
        EndOfLevelHandler.DetectedPlayer += OnPlayerDetected;
        _channel.Raised += OnLoaded;
        
        _scoreText.enabled = true;
        _totalScore.enabled = false;
        s_scoreCanvas = _scoreCanvas;
        
        UpdateText();
    }

    private void OnDisable()
    {
        Game.RequestedRestart -= Restore;
        Game.RequestedNextLevel -= OnRequestedNextLevel;
        EndOfLevelHandler.DetectedPlayer -= OnPlayerDetected;
        _channel.Raised -= OnLoaded;

        s_currentStepCount = 0;
        s_wholeStepAmount = 0;
    }
    
    public static void Hide()
    {
        s_scoreCanvas.enabled = false;
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
        s_wholeStepAmount += s_currentStepCount;
        
        Restore();

        _totalScore.enabled = false;
        
        await UniTask.Yield();
    }

    private void Restore()
    {
        s_currentStepCount = 0;

        _totalScore.enabled = false;

        if (!s_scoreCanvas.isActiveAndEnabled)
            s_scoreCanvas.enabled = true;
        
        UpdateText();
    }
    
    private void OnStep()
    {
        s_currentStepCount++;
        
        UpdateText();
    }

    private void UpdateText()
    {
        _scoreText.text = $"Score: {s_currentStepCount}";
    }
    
    private void OnPlayerDetected()
    {
        _totalScoreText.SetText($"Steps this level: {s_currentStepCount}\n" +
                                  $"Steps total: {s_wholeStepAmount + s_currentStepCount}");
        
        _totalScore.enabled = true;
    }
}
