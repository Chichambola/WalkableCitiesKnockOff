using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public abstract class BasicObject : MonoBehaviour
{
    [SerializeField] protected RigidbodyType2D RigidbodyType;
    [SerializeField] private bool _isTrigger = false;
    
    protected Rigidbody2D Rigidbody;
    protected Collider Collider;
    
    protected virtual void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        Collider = GetComponent<Collider>();
    }

    protected virtual void OnValidate()
    {
        GetComponent<Rigidbody2D>().gravityScale = 0;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType;
        GetComponent<Collider2D>().isTrigger = _isTrigger;
    }
}
