using System;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Rotator2 : MonoBehaviour
{
    [SerializeField] private float _spinSpeed = 200;
    [SerializeField] private FeetHandler _feetHandler;
    [SerializeField, Range(-1,1)] private int _spinDirection;
    
    private Rigidbody2D _rigidbody;
    private int _framesSinceSwitched;
    private float _angularVelocity;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }
    
    private void FixedUpdate()
    {
        _framesSinceSwitched++;
        
        if (Mathf.Abs(_angularVelocity) > _spinSpeed * Mathf.Deg2Rad)
        {
            _angularVelocity = (_spinSpeed * Mathf.Deg2Rad) * _spinDirection;
        }

        _rigidbody.angularVelocity = _angularVelocity;
    }
}