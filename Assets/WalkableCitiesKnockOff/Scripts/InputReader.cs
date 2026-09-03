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
        _controlsMap.Disable();
    }
    
    private void OnChangeFoot(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && ctx.ReadValue<float>() >= 0.9f)
        {
            Debug.Log("1");
        }

        if (ctx.started)
        {
            Debug.Log("2");
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

    private void ChangeFootPerformed(InputAction.CallbackContext obj)
    {
        ChangeKeyPressed?.Invoke();
    }
}
