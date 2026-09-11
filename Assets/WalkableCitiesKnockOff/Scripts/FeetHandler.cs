using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeetHandler : MonoBehaviour
{
    [SerializeField] private Foot _leftFoot;
    [SerializeField] private Foot _rightFoot;

    private Vector3 _lastPosition;
    private Foot _activeFoot;
    
    private void OnValidate()
    {
        if (!_leftFoot.IsActive && !_rightFoot.IsActive)
            _rightFoot.SetActive(true);
        
        if (_leftFoot.IsActive && _rightFoot.IsActive)
            _leftFoot.SetActive(false);
    }

    public Vector3 GetPosition()
    {
        if (_leftFoot.IsActive)
        {
            _leftFoot.SetActive(false); 
            _leftFoot.ResetCharacteristics(transform);

            _activeFoot = _rightFoot;
            _rightFoot.SetActive(true);
            _lastPosition = _rightFoot.transform.position;
            
            return _rightFoot.transform.position;
        }
        else
        {
            _rightFoot.SetActive(false);
            _rightFoot.ResetCharacteristics(transform);
            
            _leftFoot.SetActive(true);
            _activeFoot = _leftFoot;
            _lastPosition = _leftFoot.transform.position;
            
            return _leftFoot.transform.position;
        }
    }

    public void DestroyActiveFoot()
    {
        if (_activeFoot != null)
            Destroy(_activeFoot.gameObject);
    }
}
