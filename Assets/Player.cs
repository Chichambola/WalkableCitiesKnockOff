using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private FeetHandler _feetHandler;
    [SerializeField] private Rotator _rotator;
    
    private Rigidbody2D _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        _inputReader.ChangeKeyPressed += OnChangeKeyPressed;
    }

    private void OnDisable()
    {
        _inputReader.ChangeKeyPressed -= OnChangeKeyPressed;
    }

    private void OnChangeKeyPressed()
    {
        Vector3 value = _feetHandler.ChangeFoot();

        var centerOfMass = new Vector2(value.x, value.y);
        
        _rigidbody.centerOfMass = centerOfMass;
    }
}
