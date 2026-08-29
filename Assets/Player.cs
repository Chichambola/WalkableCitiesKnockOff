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

    public event Action RequestedRestart;
    
    private Rigidbody2D _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnValidate()
    {
        GetComponent<Rigidbody2D>().gravityScale = 0;
    }

    private void OnEnable()
    {
        _inputReader.ChangeKeyPressed += OnChangeKeyPressed;
        _inputReader.ResetPressed += OnResetPressed;
    }
    
    private void OnDisable()
    {
        _inputReader.ChangeKeyPressed -= OnChangeKeyPressed;
        _inputReader.ResetPressed -= OnResetPressed;
    }

    
    private void OnChangeKeyPressed()
    {
        _rigidbody.Sleep();
        
        Vector3 value = _feetHandler.GetPosition();
        _rotator.SetPoint(value);
        _rotator.SwitchPosition();
        
        _rigidbody.WakeUp();
    }

    private void OnResetPressed() => RequestedRestart?.Invoke();
}
