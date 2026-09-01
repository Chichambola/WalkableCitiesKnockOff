using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Foot : MonoBehaviour
{
    [SerializeField] private bool _isActive;
    
    private Collider2D _collider;
    private Vector3 _initialPosition;
    private Quaternion _defaultRotation = new (0f, 0f, 0f,0f);

    public bool IsActive => _isActive;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
    }

    private void OnEnable()
    {
        _initialPosition = transform.localPosition;
    }

    private void FixedUpdate()
    {
        if (!_isActive)
        {
            transform.localPosition = _initialPosition;
        }
    }

    public void SetActive(bool value)
    {
        _isActive = value;

        _collider.enabled = !value;
        
        transform.parent = null;
    }

    public void ResetCharacteristics(Transform parent)
    {
        transform.parent = parent;
        transform.rotation = _defaultRotation;
    }
}
