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

public class FeetHandler : MonoBehaviour
{
    [SerializeField] private Foot _leftFoot;
    [SerializeField] private Foot _rightFoot;
    
    private Foot _activeFoot;
    private Foot _inactiveFoot;
    private Vector3 _lastPosition;
    private Quaternion _lastRotation;
    
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
    
    public Vector2 GetPosition() => _activeFoot.Position;
    public float GetRotation() => _activeFoot.Rotation;
    
    public void Switch()
    {
        _activeFoot.SetActive(false);
        _activeFoot.ResetCharacteristics();
        
        _inactiveFoot.SetActive(true);

        var tempActive  = _activeFoot;
        var tempInactive = _inactiveFoot;
        
        _activeFoot = tempInactive;
        _inactiveFoot = tempActive;
    }

    public void ResetFeet()
    {
        _activeFoot.ResetCharacteristics();
        _inactiveFoot.ResetCharacteristics();
    }
}