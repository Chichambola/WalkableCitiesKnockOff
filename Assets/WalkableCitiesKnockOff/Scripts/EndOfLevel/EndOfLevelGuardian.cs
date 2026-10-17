using System;
using System.Collections;
using System.Collections.Generic;
using AYellowpaper;
using PrimeTween;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class EndOfLevelGuardian : MonoBehaviour
{
    [SerializeField] private BoxCollider2D _blockCollider;
    [SerializeField] private InterfaceReference<ITaskHandler, MonoBehaviour> _taskHandlerPrefab;
    [Header("Tween settings")]
    [SerializeField] private float _offset = 10f;
    [SerializeField] private float _duration = 1.5f;
    [SerializeField] private bool _isXAxis = true;

    private ITaskHandler _taskHandler;
    private TweenSettings<float> _settings;

    private void Awake()
    {
        _taskHandler = _taskHandlerPrefab.Value;
    }

    private void OnEnable()
    {
        _taskHandler.Completed += OnComplete;

        if (_isXAxis)
        {
            _settings.endValue = transform.position.x + _offset;
            _settings.startValue = transform.position.x;
        }
        else
        {
            _settings.endValue = transform.position.y + _offset;
            _settings.startValue = transform.position.y;
        }

        _settings.settings.duration = _duration;
    }

    private void OnDisable()
    {
        _taskHandler.Completed -= OnComplete;
    }

    private void OnComplete()
    {
        _blockCollider.enabled = false;

        if (_isXAxis)
        {
            Tween.PositionX(transform, _settings);
        }
        else
        {
            Tween.PositionY(transform, _settings);
        }
    }
}