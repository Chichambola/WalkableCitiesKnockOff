using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Rotator : MonoBehaviour
{
    [SerializeField] private float _spinSpeed = 2;
    [SerializeField] private Vector2 _spinDirection = Vector2.right;

    private Rigidbody2D _rigidbody;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        _rigidbody.angularVelocity += _spinSpeed * Mathf.Deg2Rad;
    }
}
