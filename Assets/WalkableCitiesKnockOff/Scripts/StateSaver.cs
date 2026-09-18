using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class StateSaver : Singleton<StateSaver>
{
    [SerializeField] private float _initialInterval = .5f;

    private static Dictionary<ICapturable, CapturableState> s_capturables;
    private CancellationTokenSource _cts;
    private static float _currentInterval;

    protected override void Awake()
    {
        base.Awake();
        
        _currentInterval = _initialInterval;
        
        s_capturables = new Dictionary<ICapturable, CapturableState>();
    }

    private void OnEnable()
    {
        Game.RequestedRestart += OnRestart;
        
        _cts = new CancellationTokenSource();
        
        CaptureLastStateTask(_cts.Token).Forget();
    }

    private void OnDisable()
    {
        Game.RequestedRestart -= OnRestart;
    }

    private void OnDestroy()
    {
        _cts?.Cancel();
        _cts?.Dispose();
        
        s_capturables.Clear();
    }

    private void OnRestart()
    {
        _cts?.Cancel();
        
        s_capturables.Clear();

        _cts = new CancellationTokenSource();
        
        CaptureLastStateTask(_cts.Token).Forget();
    }
    
    public static void Register(ICapturable capturable)
    {
        CapturableState capturableState = new CapturableState(capturable.Position, capturable.Rotation);
        
        s_capturables.Add(capturable, capturableState);
    }

    public static CapturableState GetState(ICapturable capturable)
    {
        if (!s_capturables.ContainsKey(capturable))
            throw new ExternalException($"This object was not registered!");
        
        var value = s_capturables.GetValueOrDefault(capturable);
        
        return value;
    }
    
    private async UniTask CaptureLastStateTask(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            if (s_capturables.Count == 0)
                await UniTask.Delay(TimeSpan.FromSeconds(_currentInterval), cancellationToken: token);

            var kvpList = s_capturables.ToList();
            
            foreach (var kvp in kvpList)
            {
                if (!kvp.Key.IsStuck)
                {
                    s_capturables[kvp.Key] = new CapturableState(kvp.Key.Position, kvp.Key.Rotation);
                }
            }

            await UniTask.Delay(TimeSpan.FromSeconds(_currentInterval), cancellationToken: token);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        
        foreach (var kvp in s_capturables.Values)
        {
            Gizmos.DrawSphere(kvp.Position, 1f);
        }
    }
}