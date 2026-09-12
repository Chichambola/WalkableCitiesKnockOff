using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading;
using Cysharp.Threading.Tasks;
using PrimeTween;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SliderHandler : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private float _duration = 1f;
    [SerializeField] private float _increaseValue = 0.05f;

    public event Action ReachedMaxValue;
    
    private CancellationTokenSource _cts;
    private TweenSettings<float> _slidersSettings;
    private bool _isGaining;
    private Tween _tween;

    private void Awake()
    {
        _slidersSettings = new TweenSettings<float>();

        _slidersSettings.settings.duration = _duration;
        _slidersSettings.settings.useUnscaledTime = true;
    }

    private void OnDisable()
    {
        _cts?.Cancel();
    }

    private void OnDestroy()
    {
        _cts?.Dispose();
    }

    private void Update()
    {
        Debug.Log(_slider.value);
    }

    public void StartMoving()
    {
        _cts = new CancellationTokenSource();
        _cts.RegisterRaiseCancelOnDestroy(this);
        
        MoveSliderTask(_cts.Token).Forget();
    }

    public void StopMoving()
    {
        _cts?.Cancel();
        
        _slidersSettings.startValue = _slider.value;
        _slidersSettings.endValue = 0f;
        
        Tween.UISliderValue(_slider, _slidersSettings);
    }
    
    public void SetGainingStatus(bool value)
    {
        _isGaining = value;
    }

    private async UniTask MoveSliderTask(CancellationToken token)
    {
        float value = 0f;
        _slider.gameObject.SetActive(true);
        
        while (!_cts.IsCancellationRequested)
        {
            if (_isGaining)
            {
                value += _increaseValue;
            }
            else
            {
                value -= _increaseValue;

                if (value <= 0)
                {
                    value = 0;
                }
            }
            
            _slidersSettings.startValue = _slider.value;
            _slidersSettings.endValue = value;

            Tween.UISliderValue(_slider, _slidersSettings);
            
            await UniTask.Delay(TimeSpan.Zero, true, cancellationToken: token);

            if (!Mathf.Approximately(_slider.value, _slider.maxValue))
                continue;
                
            ReachedMaxValue?.Invoke();
        }
        
        _cts?.Cancel();
    }
}
