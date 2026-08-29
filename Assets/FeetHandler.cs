using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeetHandler : MonoBehaviour
{
    [SerializeField] private Foot _leftFoot;
    [SerializeField] private Foot _rightFoot;

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
            
            _rightFoot.SetActive(true);
            
            return _rightFoot.transform.position;
        }
        else
        {
            _rightFoot.SetActive(false);
            
            _leftFoot.SetActive(true);
            
            return _leftFoot.transform.position;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_rightFoot.transform.position, 0.3f);
        
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_leftFoot.transform.position, 0.3f);
    }

    public Vector3 GetDirection()
    {
        if (_rightFoot.IsActive)
        {
            return _rightFoot.transform.position;
        }
        else
        {
            return _leftFoot.transform.position;
        }
    }
}
