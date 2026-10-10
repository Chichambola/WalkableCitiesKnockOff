using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class EndOfLevelHandler : Singleton<EndOfLevelHandler>
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private SliderHandler _sliderHandler;
    [SerializeField] private bool _isImmediateLevelChanging = false;

    public static event Action RequestedEndOfLevel;
    public static event Action DetectedPlayer;
    
    private IPlayer _player;
    private CancellationTokenSource _cts;
    private Vector3 _initialPosition;
    private Vector3 _outOfCameraPosition = new (99999, 99999);
    private static EndOfLevel s_endOfLevel;
    
    public static void Init(EndOfLevel endOfLevel, EndOfLevelHandler instance)
    {
        if (s_endOfLevel != null)
        {
            s_endOfLevel.PlayerDetected -= instance.Process;
            s_endOfLevel = null;
        }

        s_endOfLevel = endOfLevel;
        s_endOfLevel.PlayerDetected += instance.Process;
    }

    protected override void Awake()
    {
        base.Awake();

        _initialPosition = transform.position;
    }

    private void OnEnable()
    {
        _sliderHandler.ReachedMaxValue += OnReachedMaxValue;
        Game.RequestedRestart += OnRequestedRestart;
        
        SetActive(false);
    }

    private void OnDisable()
    {
        _sliderHandler.ReachedMaxValue -= OnReachedMaxValue;
        Game.RequestedRestart -= OnRequestedRestart;
        
        if (s_endOfLevel != null)
        {
            var instance = Instance as EndOfLevelHandler;

            s_endOfLevel.PlayerDetected -= instance.Process;
        }
    }

    private void Process(IPlayer player)
    {
        _player = player;
        
        if (!_isImmediateLevelChanging)
        {
            Game.Pause();
            
            SetActive(true);
            
            DetectedPlayer?.Invoke();
            
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

        SetActive(false);
        
        RequestedEndOfLevel?.Invoke();
    }
    
    private void OnRequestedRestart()
    {
        SetActive(false);
    }

    private void SetActive(bool value)
    {
        if (!value)
        {
            _cts?.Cancel();
            
            _sliderHandler.StopMoving();
        }

        _canvas.enabled = value;
    }
}



