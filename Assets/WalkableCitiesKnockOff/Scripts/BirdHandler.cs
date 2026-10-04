using System;
using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;

public class BirdHandler : MonoBehaviour, ITaskHandler
{
    [SerializeField] private List<Bird> _birds;
    [SerializeField] private BirdDetector _detector;

    public event Action Completed;
    private int _birdsCount;
    
    private void OnEnable()
    {
        _detector.Found += OnFound;

        _birdsCount = _birds.Count;
    }

    private void OnDisable()
    {
        _detector.Found -= OnFound;
    }

    private void OnFound(Bird bird)
    {
        bird.gameObject.SetActive(false);

        _birdsCount--;

        if (_birdsCount == 0)
        {
            Completed?.Invoke();
        }
    }
}