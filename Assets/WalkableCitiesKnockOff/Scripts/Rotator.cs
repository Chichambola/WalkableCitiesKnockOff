using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Rotator : MonoBehaviour
{
    [SerializeField] private float _spinSpeed = 200;
    [SerializeField] private float _stuckRotationThreshold;
    [SerializeField] private int _stuckFramesThreshold = 10;
    [SerializeField] private Transform _rotatePoint;

    private bool _isFlipped;
    private int _spinDirection;
    private int _forward = 1;
    private int _backward = -1;
    private int _count;
    private int _stuckFrames;
    private float _currentRotation;
    private float _lastRotation;
    private Vector2 _currentPosition;
    private Rigidbody2D _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _rigidbody.centerOfMass = Vector2.zero;
    }
    
    private void FixedUpdate()
    {
        if (_spinDirection == 0)
            return;
        
        _currentPosition = _rigidbody.position;
        _currentRotation = _rigidbody.rotation;

        float actualDelta = Mathf.Abs(Mathf.DeltaAngle(_currentRotation, _lastRotation));
        
        float expectedDelta = Mathf.Abs(_spinSpeed * Time.fixedDeltaTime);

        if (actualDelta < expectedDelta * _stuckRotationThreshold)
        {
            _stuckFrames++;
        }
        else
        {
            _stuckFrames = 0;
        }
        
        if (_stuckFrames >= _stuckFramesThreshold)
        {
            SwitchDirection();
            _stuckFrames = 0;
        }
        
        _lastRotation = _currentRotation;
        
        Vector2 offset = _currentPosition - (Vector2)_rotatePoint.position;

        float angleDelta = _spinSpeed * Time.fixedDeltaTime * _spinDirection;
        
        Vector2 rotatedOffset = Quaternion.Euler(0,0 ,angleDelta ) * offset;

        var nextPos = (Vector2)_rotatePoint.position + rotatedOffset;

        var nextAngle = _currentRotation + angleDelta;
        
        _rigidbody.MovePositionAndRotation(nextPos, nextAngle);
    }
    
    public void SwitchPosition()
    {
        if (_isFlipped)
        {
            _spinDirection = _forward;
            _isFlipped = false;
        }
        else
        {
            _isFlipped = true;
            _spinDirection = _backward;
        }
    }
    
    public void SwitchDirection()
    {
        _spinDirection = _spinDirection == _backward ? _forward : _backward;
    }
    
    public void SetPoint(Vector2 point)
    {
        _rotatePoint.position = point;
    }

    public void ResetCharacteristics()
    {
        SetPoint(Vector2.zero);
        _spinDirection = 0;
        _isFlipped = true;
    }
}