using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Rotator : MonoBehaviour
{
    [SerializeField] private float _spinSpeed = 2;

    private bool _isFlipped;
    private Rigidbody2D _rigidbody;
    private Vector3 _rotatePoint;
    private Vector3 _spinDirection;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (!_rigidbody.IsSleeping())
        {
            _rigidbody.transform.RotateAround(_rotatePoint, _spinDirection, _spinSpeed * Time.fixedDeltaTime);
        }
    }

    public void SwitchDirection()
    {
        if (_isFlipped)
        {
            _spinDirection = Vector3.back;
            _isFlipped = false;
        }
        else
        {
            _isFlipped = true;
            _spinDirection = Vector3.forward;
        }
    }
    
    public void SetPoint(Vector3 point) => _rotatePoint = point;
}
