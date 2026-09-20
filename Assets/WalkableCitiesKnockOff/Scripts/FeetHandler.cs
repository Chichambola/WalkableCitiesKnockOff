using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class FeetHandler : MonoBehaviour, IStuckDetector
{
    [SerializeField] private Foot _leftFoot;
    [SerializeField] private Foot _rightFoot;
    
    private Foot _activeFoot;
    private Foot _inactiveFoot;
    private Vector3 _lastPosition;
    private Quaternion _lastRotation;
    private IStuckDetector _stuckDetector;

    public Vector2 Position => _activeFoot.Position;
    public bool IsStuck => _stuckDetector.IsStuck;

    public void Init(IStuckDetector stuckDetector)
    {
        _stuckDetector = stuckDetector;
    }
    
    private void Awake()
    {
        if (_leftFoot.IsActive)
        {
            _activeFoot = _leftFoot;
            _inactiveFoot = _rightFoot;
        }
        else
        {
            _activeFoot = _rightFoot;
            _inactiveFoot = _leftFoot;
        }
        
        _activeFoot.Init(this);
        _inactiveFoot.Init(this);
    }

    private void OnValidate()
    {
        if (!_leftFoot.IsActive && !_rightFoot.IsActive)
            _rightFoot.SetActive(true);
        
        if (_leftFoot.IsActive && _rightFoot.IsActive)
            _leftFoot.SetActive(false);
    }

    private void OnDestroy()
    {
        if (_activeFoot != null)
            Destroy(_activeFoot.gameObject);
    }
    
    public Vector3 GetPosition()
    {
        Vector3 position = _activeFoot.Position;

        return position;
    }

    public void Set(Vector3 pos)
    {
        _activeFoot.transform.position = pos;
    }

    public void ResetToLastState(Vector2 offset)
    {
        _activeFoot.ResetToLastState(offset);
        _inactiveFoot.ResetToLastState(offset);
    }
    
    public void Switch()
    {
        _activeFoot.SetActive(false);
        _activeFoot.ResetCharacteristics(transform);
        
        _inactiveFoot.SetActive(true);

        var tempActive  = _activeFoot;
        var tempInactive = _inactiveFoot;
        
        _activeFoot = tempInactive;
        _inactiveFoot = tempActive;
    }
}