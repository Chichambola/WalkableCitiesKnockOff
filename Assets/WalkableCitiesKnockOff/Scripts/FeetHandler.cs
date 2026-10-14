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
    private bool _isLeftInitialFoot;
    
    private void Awake()
    {
        if (_leftFoot.IsActive)
        {
            _activeFoot = _leftFoot;
            _inactiveFoot = _rightFoot;
            _isLeftInitialFoot = true;
        }
        else
        {
            _activeFoot = _rightFoot;
            _inactiveFoot = _leftFoot;
            _isLeftInitialFoot = false;
        }

        if (!_leftFoot.IsActive && !_rightFoot.IsActive)
        {
            _rightFoot.SetActive(true);
            _activeFoot = _rightFoot;
            _inactiveFoot = _leftFoot;
        }
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
        _activeFoot   = _isLeftInitialFoot ? _leftFoot : _rightFoot;
        _inactiveFoot = _isLeftInitialFoot ? _rightFoot : _leftFoot;

        _activeFoot.SetActive(true);
        _inactiveFoot.SetActive(false);
        
        _activeFoot.ResetCharacteristics();
        _inactiveFoot.ResetCharacteristics();
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
            return;
        
        Gizmos.color =Color.red;
        
        Gizmos.DrawSphere(_activeFoot.Position, 1f);
    }
}