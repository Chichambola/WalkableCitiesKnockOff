using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

public class InputReader : MonoBehaviour
{
    public event Action ChangeKeyPressed;
    public event Action ResetPressed;
    
    private ControlsMap _controlsMap;
    private bool _isPressed; 
    

    private void Awake()
    {
        _controlsMap = new ControlsMap();
    }

    private void OnEnable()
    {
        _controlsMap.Enable();

        _controlsMap.Controls.ChangeFoot.performed += OnChangeFoot;
    }

    private void OnDisable()
    {
        _controlsMap.Controls.ChangeFoot.performed -= OnChangeFoot;
        
        _controlsMap.Disable();
    }
    
    private void OnChangeFoot(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            ChangeKeyPressed?.Invoke();
        }
    }
    
    private void Update()
    {

        
        return;
        
        if (_controlsMap.Controls.ChangeFoot.WasPerformedThisFrame())
            ChangeKeyPressed?.Invoke();

        if (_controlsMap.Controls.Restart.WasPressedThisFrame())
            ResetPressed?.Invoke();
    }
}
