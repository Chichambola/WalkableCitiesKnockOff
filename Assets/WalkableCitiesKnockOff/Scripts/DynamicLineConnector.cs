using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.iOS;

[RequireComponent(typeof(LineRenderer))]
public class DynamicLineConnector : MonoBehaviour
{
    [SerializeField] private Transform _aPoint;
    [SerializeField] private Transform _bPoint;
    
    private LineRenderer _lineRenderer;
    
    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (_aPoint == null || _bPoint == null)
            return;

        Vector3 pos = new Vector3(_aPoint.position.x, _aPoint.position.y, transform.position.z);
        _lineRenderer.SetPosition(0, pos);

        pos = new Vector3(_bPoint.position.x, _bPoint.position.y, transform.position.z);
        _lineRenderer.SetPosition(1, pos);
    }
}
