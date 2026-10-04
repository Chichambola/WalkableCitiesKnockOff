using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class VelocityVisualizer : MonoBehaviour
{
    [SerializeField, Range(0.5f, 2f)] private float _arrowLength;
    
    private Rigidbody2D _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
            return;

        var position = transform.position;
        var velocity = _rigidbody.velocity;

        if (velocity.magnitude < 0.1f)
            return;
        
        
        Handles.color = Color.red;
        Handles.ArrowHandleCap(0, position, Quaternion.LookRotation(velocity), _arrowLength, EventType.Repaint);
    }
}
