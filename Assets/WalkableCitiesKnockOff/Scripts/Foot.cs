using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Foot : MonoBehaviour
{
    [SerializeField] private bool _isActive;
    
    private Collider2D _collider;
    private Quaternion _defaultRotation = new (0f, 0f, 0f,0f);
    private Quaternion _currentRotation;
    private Vector3 _currentPosition;

    public Vector2 Position => transform.position;
    public bool IsActive => _isActive;
    public float Rotation => transform.rotation.eulerAngles.z;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
    }
    
    private void FixedUpdate()
    {
         transform.rotation = _currentRotation;
    }

    public void SetActive(bool value)
    {
        _currentRotation = value ? transform.rotation : _defaultRotation;
        
        _isActive = value;

        //_collider.enabled = !value;
    }
    
    public void ResetCharacteristics()
    {
        _currentRotation = _defaultRotation;
    }
}
