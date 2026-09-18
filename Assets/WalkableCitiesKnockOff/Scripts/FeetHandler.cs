using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEditorInternal;
using UnityEngine;
using Random = UnityEngine.Random;

public class FeetHandler : MonoBehaviour
{
    [SerializeField] private Foot _leftFoot;
    [SerializeField] private Foot _rightFoot;
    
    private Foot _activeFoot;
    private Foot _inactiveFoot;
    private Vector3 _lastPosition;
    private Quaternion _lastRotation;
    private IStuckDetector _stuckDetector;

    public Vector2 Position => _activeFoot.Position;
    public Quaternion Rotation => _activeFoot.Rotation;
    public bool IsStuck => _stuckDetector.IsStuck;

    public void Init(IStuckDetector stuckDetector)
    {
        _stuckDetector = stuckDetector;
    }
    
    private void Awake()
    {
        _activeFoot = _leftFoot.IsActive ? _leftFoot : _rightFoot;
    }

    private void OnEnable()
    {
        StateSaver.Register(this);
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
        Vector3 position;
        
        if (_leftFoot.IsActive)
        {
            _leftFoot.SetActive(false); 
            _leftFoot.ResetCharacteristics(transform);

            _rightFoot.SetActive(true);
            
            _activeFoot = _rightFoot;
            _inactiveFoot = _leftFoot;
            
            position = _rightFoot.Position;
        }
        else
        {
            _rightFoot.SetActive(false);
            _rightFoot.ResetCharacteristics(transform);
            
            _leftFoot.SetActive(true);
            
            _activeFoot = _leftFoot;
            _inactiveFoot = _rightFoot;
            
            position = _leftFoot.Position;
        }
        
        return position;
    }

    public void Set(Vector3 pos)
    {
        _activeFoot.transform.position = pos;
    }

    public void ResetToLastState()
    {
        var value = StateSaver.GetState(this);
        
        _activeFoot.transform.position = value.Position;
        _activeFoot.transform.rotation = value.Rotation;
    }
}