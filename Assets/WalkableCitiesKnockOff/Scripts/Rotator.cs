using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Rotator : MonoBehaviour, ICapturable
{
    [SerializeField] private float _spinSpeed = 200;

    private bool _isFlipped;
    private IStuckDetector _stuckDetector;
    private Rigidbody2D _rigidbody;
    private Vector2 _rotatePoint;
    private Vector3 _spinDirection;
    
    public Vector2 Position => _rotatePoint;
    public Quaternion Rotation => transform.rotation;
    public bool IsStuck => _stuckDetector.IsStuck;

    public void Init(IStuckDetector stuckDetector)
    {
        _stuckDetector = stuckDetector;
    }
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        StateSaver.Register(this);   
    }

    private void FixedUpdate()
    {
        transform.RotateAround(_rotatePoint, _spinDirection, _spinSpeed * Time.fixedDeltaTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawSphere(_rotatePoint, 1f);
    }

    public void SwitchPosition()
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
    
    
    public void SwitchDirection()
    {
        _spinDirection = _spinDirection == Vector3.back ? Vector3.forward : Vector3.back;
    }
    
    public void SetPoint(Vector3 point) => _rotatePoint = point;

    public void ResetToLastState(Vector2 offset)
    {
        var value = StateSaver.GetState(this);
        
        SetPoint(value.Position);
    }
}