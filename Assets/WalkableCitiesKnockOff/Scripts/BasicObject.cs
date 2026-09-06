using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public abstract class BasicObject : MonoBehaviour
{
    [SerializeField] private RigidbodyType2D _type;
    
    protected Rigidbody2D Rigidbody;
    private Collider _collider;
    
    protected virtual void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider>();
    }

    private void OnValidate()
    {
        GetComponent<Rigidbody2D>().gravityScale = 0;
        GetComponent<Rigidbody2D>().bodyType = _type;
    }
}
