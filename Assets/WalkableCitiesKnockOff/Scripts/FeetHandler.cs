using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class FeetHandler : MonoBehaviour
{
    [SerializeField] private Foot _leftFoot;
    [SerializeField] private Foot _rightFoot;
    [SerializeField] private StepVerifier _stepVerifier;
    [SerializeField] private int _recordStepFrameCount = 15;

    private Foot _activeFoot;
    private Foot _inactiveFoot;
    private Vector3 _lastPosition;
    private int _startFrame;

    private void Start()
    {
        _startFrame = Time.frameCount;
    }

    private void Update()
    {
        if (_inactiveFoot == null)
            return;
        
        int framesPassed = Time.frameCount - _startFrame;
        
        if (framesPassed >= _recordStepFrameCount)
        {
            _lastPosition = _inactiveFoot.Position;
        }
    }

    private void OnValidate()
    {
        if (!_leftFoot.IsActive && !_rightFoot.IsActive)
            _rightFoot.SetActive(true);
        
        if (_leftFoot.IsActive && _rightFoot.IsActive)
            _leftFoot.SetActive(false);
    }

    private void OnDestroy()
    {
        if (_activeFoot != null)
            Destroy(_activeFoot.gameObject);
    }

    public Vector3 GetPosition()
    {
        Vector3 position;

        if (_inactiveFoot != null && !_stepVerifier.CanPlace(_inactiveFoot.Position, _inactiveFoot.Size, _inactiveFoot.ZAngle))
        {
            _inactiveFoot.Set(_lastPosition);
        }
        
        if (_leftFoot.IsActive)
        {
            _leftFoot.SetActive(false); 
            _leftFoot.ResetCharacteristics(transform);

            _rightFoot.SetActive(true);
            
            _activeFoot = _rightFoot;
            _inactiveFoot = _leftFoot;
            
            position = _rightFoot.Position;
        }
        else
        {
            _rightFoot.SetActive(false);
            _rightFoot.ResetCharacteristics(transform);
            
            _leftFoot.SetActive(true);
            
            _activeFoot = _leftFoot;
            _inactiveFoot = _rightFoot;
            
            position = _leftFoot.Position;
        }

        return position;
    }
}