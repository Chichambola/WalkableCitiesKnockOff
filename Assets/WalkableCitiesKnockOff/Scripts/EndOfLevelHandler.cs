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

public class EndOfLevelHandler : Singleton<EndOfLevelHandler>
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private SliderHandler _sliderHandler;
    [SerializeField] private bool _isImmediateLevelChanging = false;

    public static event Action RequestedEndOfLevel;
    
    private IPlayer _player;
    private CancellationTokenSource _cts;
    private static EndOfLevel s_endOfLevel;
    
    public static void Init(EndOfLevel endOfLevel) => s_endOfLevel = endOfLevel;
    
    protected void Awake()
    {
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
    
    private void Process(IPlayer player)
    {
        _player = player;
        
        if (!_isImmediateLevelChanging)
        {
            Game.Pause();

            _text.text = $"Hold {_player.AcceptButton.ToUpper()} to proceed to the next level\n" +
                         $"Press {_player.RestartButton.ToUpper()} to restart level";
            
            _cts = new CancellationTokenSource();
            _cts.RegisterRaiseCancelOnDestroy(this);

            IsPlayerHoldingButton(_cts.Token).Forget();

            _sliderHandler.StartMoving();
        }
        else
        {
            RequestedEndOfLevel?.Invoke();
        }
    }

    private async UniTaskVoid IsPlayerHoldingButton(CancellationToken token)
    {
        while (!_cts.IsCancellationRequested)
        {
            _sliderHandler.SetGainingStatus(_player.IsAccepting);

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
