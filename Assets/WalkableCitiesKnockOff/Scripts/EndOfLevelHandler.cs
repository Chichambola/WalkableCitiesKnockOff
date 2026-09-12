using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class EndOfLevelHandler : Singleton<EndOfLevelHandler>
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private SliderHandler _sliderHandler;
    [SerializeField] private bool _isImmediateLevelChanging = false;

    public static event Action RequestedEndOfLevel;
    
    private IPlayer _player;
    private CancellationTokenSource _cts;
    private Vector3 _initialPosition;
    private Vector2 _outOfCameraPosition = new Vector2(99999, 99999);
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
        
        _text.text = "";

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

            _text.text = $"Hold {_player.AcceptButton.ToUpper()} to proceed to the next level\n" +
                         $"Press {_player.RestartButton.ToUpper()} to restart level";
            
            SetActive(true);
            
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

            transform.position = _outOfCameraPosition;
        }
        else
        {
            transform.position = _initialPosition;
        }
    }
}



