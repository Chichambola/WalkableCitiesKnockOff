using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class StateSaver : Singleton<StateSaver>
{
    [SerializeField] private float _interval = .5f;

    private static Dictionary<ICapturable, CapturableState> s_capturables = new ();
    private CancellationTokenSource _cts;
    
    private void OnEnable()
    {
        _cts = new CancellationTokenSource();
        
        CaptureLastStateTask(_cts.Token).Forget();
    }

    private void OnDestroy()
    {
        _cts?.Cancel();
        _cts?.Dispose();
    }

    public static void Register(ICapturable capturable)
    {
        CapturableState capturableState = new CapturableState(capturable.Position, capturable.Rotation);
        
        s_capturables.Add(capturable, capturableState);
    }

    public static CapturableState GetState(ICapturable capturable)
    {
        if (!s_capturables.TryGetValue(capturable, out var value))
            throw new ExternalException($"This object was not registered!");

        return value;
    }
    
    private async UniTask CaptureLastStateTask(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            if (s_capturables.Count == 0)
                await UniTask.Delay(TimeSpan.FromSeconds(_interval), cancellationToken: token);

            foreach (var kvp in s_capturables)
            {
                s_capturables[kvp.Key] = new CapturableState(kvp.Key.Position, kvp.Key.Rotation);
            }

            await UniTask.Delay(TimeSpan.FromSeconds(_interval), cancellationToken: token);
        }
    }
}