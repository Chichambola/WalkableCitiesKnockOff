using System;
using TMPro;
using UnityEngine;

public class Gates : BasicObject, ITaskHandler
{
    [SerializeField] private BallDetector _ballDetector;
    
    public event Action Completed;

    private bool _isCompleted;
    
    private void OnEnable()
    {
        _ballDetector.Found += OnFound;
        
        _isCompleted = false;
    }

    private void OnDisable()
    {
        _ballDetector.Found -= OnFound;

        _isCompleted = false;
    }

    private void OnFound(Ball ball)
    {
        if (_isCompleted)
            return;
        
        Completed?.Invoke();

        _isCompleted = true;
    }
}