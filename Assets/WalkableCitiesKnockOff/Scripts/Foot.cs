using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Foot : MonoBehaviour, ICapturable
{
    [SerializeField] private bool _isActive;
    
    private Collider2D _collider;
    private Vector3 _initialPosition;
    private Quaternion _defaultRotation = new (0f, 0f, 0f,0f);
    private IStuckDetector _stuckDetector;

    public Vector2 Position => transform.position;
    public Quaternion Rotation => transform.rotation;
    public bool IsActive => _isActive;
    public bool IsStuck => _stuckDetector.IsStuck;

    public void Init(IStuckDetector stuckDetector)
    {
        _stuckDetector = stuckDetector;
    }
    
    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
    }

    private void OnEnable()
    {
        StateSaver.Register(this);
    }

    public void SetActive(bool value)
    {
        _isActive = value;

       // _collider.enabled = !value;
        
        transform.parent = null;
    }
    
    public void ResetToLastState()
    {
        var value = StateSaver.GetState(this);

        transform.position = value.Position;
        transform.rotation = value.Rotation;
    }
    
    public void ResetCharacteristics(Transform parent)
    {
        transform.parent = parent;
        transform.rotation = _defaultRotation;
    }
}
