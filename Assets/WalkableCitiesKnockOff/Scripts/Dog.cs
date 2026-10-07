using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using PrimeTween;
using UnityEngine;
using UnityEngine.InputSystem.iOS;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

public class Dog : BasicObject
{
    [SerializeField] private BirdSearcher _birdSearcher;
    [SerializeField] private Kicker _kicker;
    [Header("Tween settings")]
    [SerializeField] private float _interval = 2f;
    [SerializeField] private float _duration = 1.5f;
    [SerializeField] private Ease _ease;

    private CancellationTokenSource _cts;
    private TweenSettings<Vector2> _settings;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.TryGetComponent(out IKickable kickable))
        {
            kickable.ChangeDirection(other.contacts[0].point);
        }
    }

    private void OnEnable()
    {
        _cts = new CancellationTokenSource();

        _settings.settings.duration = _duration;
        
        LookForTask(_cts.Token).Forget();
    }

    private void OnDestroy()
    {
        _cts?.Dispose();
    }
    
    private async UniTaskVoid LookForTask(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_interval), cancellationToken: token);

            if (!_birdSearcher.TryFind(out var hits))
                continue;

            var bird = _birdSearcher.FindClosest(hits);

            _settings.startValue = transform.position;
            _settings.endValue = bird.transform.position;
            _settings.settings.ease = _ease;
            
            await Sequence.Create().Group(Tween.RigidbodyMovePosition(Rigidbody, _settings)).ToYieldInstruction().WithCancellation(token);
        }
    }
}