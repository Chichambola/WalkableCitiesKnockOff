using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foot : MonoBehaviour
{
    [SerializeField] private bool _isActive;
    
    public bool IsActive => _isActive;

    public void SetActive(bool value)
    {
        _isActive = value;
    }
}
