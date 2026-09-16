using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class StateSaver : MonoBehaviour
{
    [SerializeField] private float _interval = .5f;
    
    private ICapturable _capturable;
    private CancellationTokenSource _cts;
    private Vector3 _lastPosition;
    private Quaternion _lastRotation;

    private void OnDestroy()
    {
        _cts?.Cancel();
        _cts?.Dispose();
    }

    public void Execute(ICapturable capturable)
    {
        _capturable = capturable;
        
        _cts = new CancellationTokenSource();
        
        CaptureLastStateTask(_cts.Token).Forget();
    }
    
    public Vector3 GetPosition() => _lastPosition;
    
    public Quaternion GetRotation() => _lastRotation;
    
    private async UniTask CaptureLastStateTask(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_interval), cancellationToken: token);
        
            _lastPosition = _capturable.Position;
            _lastRotation = _capturable.Rotation;
        }
    }
}