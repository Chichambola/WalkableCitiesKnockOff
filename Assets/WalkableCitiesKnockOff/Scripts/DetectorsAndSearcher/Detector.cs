using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Detector<T> : MonoBehaviour where T : MonoBehaviour
{
    public event Action<T> Found;

    private BoxCollider2D _collider;
    
    private void Awake()
    {
        _collider = GetComponent<BoxCollider2D>();
    }

    private void OnValidate()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out T @object))
        {
            Found?.Invoke(@object);
        }
    }
}