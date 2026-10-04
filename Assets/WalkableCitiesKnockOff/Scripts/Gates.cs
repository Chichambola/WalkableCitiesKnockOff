using System;
using UnityEngine;

public class Gates : BasicObject, ITaskHandler
{
    [SerializeField] private BallDetector _ballDetector;
    
    public event Action Completed;
    
    private void OnEnable()
    {
        _ballDetector.Found += OnFound;
    }

    private void OnDisable()
    {
        _ballDetector.Found -= OnFound;
    }

    private void OnFound(Ball ball)
    {
        Completed?.Invoke();
    }
}