using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using AYellowpaper;
using Cysharp.Threading.Tasks;
using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using Slider = UnityEngine.UI.Slider;

public class EndOfLevelHandler : BasicObject
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private SliderHandler _sliderHandler;

    public event Action RequestedEndOfLevel;
    
    private IPlayer _player;
    private CancellationTokenSource _cts;
    
    protected override void Awake()
    {
        base.Awake();

        _text.text = "";
    }

    private void OnEnable()
    {
        _sliderHandler.ReachedMaxValue += OnReachedMaxValue;
    }

    private void OnDisable()
    {
        _sliderHandler.ReachedMaxValue -= OnReachedMaxValue;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent(out IPlayer player))
            return;
        
        _player = player;
            
        ProcessCollision();
    }

    private void ProcessCollision()
    {
        Game.Pause();

        _text.text = $"Hold {_player.AcceptButton.ToUpper()} to proceed to the next level\n" +
                     $"Press {_player.RestartButton.ToUpper()} to restart level";

        _cts = new CancellationTokenSource();
        _cts.RegisterRaiseCancelOnDestroy(this);
        
        IsPlayerHoldingButton(_cts.Token).Forget();
        
        _sliderHandler.StartMoving();
    }

    private async UniTaskVoid IsPlayerHoldingButton(CancellationToken token)
    {
        while (!_cts.IsCancellationRequested)
        {
            _sliderHandler.SetActive(_player.IsAccepting);

            await UniTask.Delay(TimeSpan.Zero, ignoreTimeScale: true, cancellationToken: token);
        }
        
        _cts?.Cancel();
    }
    
    private void OnReachedMaxValue()
    {
        _cts?.Cancel();

        RequestedEndOfLevel?.Invoke();
    }
}
