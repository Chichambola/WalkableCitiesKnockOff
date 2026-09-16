using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

public class StepVerifier : MonoBehaviour
{
    [SerializeField] private float _checkDistance = 1f;
    [SerializeField] private int _amountCollidersToDetect = 50;
    [SerializeField] private ContactFilter2D _filter;

    private RaycastHit2D[] _raycastHit;
    private int _hits;

    private void Awake()
    {
        _raycastHit = new RaycastHit2D[_amountCollidersToDetect];
    }

    private Vector2 _size;
    private Vector2 _pos;
    
    public bool CanPlace(Vector2 position, Vector2 size, float angle)
    {
        _hits = Physics2D.BoxCast(position, size, angle, Vector2.up, _filter, _raycastHit);
        
        return _hits == 0;
    }
}