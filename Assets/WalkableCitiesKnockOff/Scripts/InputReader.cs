using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

public class InputReader : MonoBehaviour
{
    public event Action ChangeKeyPressed;
    public event Action ResetPressed;
    
    private ControlsMap _controlsMap;
    private bool _isHolding;
    private float _holdingValue = 1f;

    public bool IsHoldingAccept => _isHolding;
    public string AcceptButton { get; private set; }
    public string RestartButton { get; private set; }
    public string ChangeFootButton { get; private set; }

    private void Awake()
    {
        _controlsMap = new ControlsMap();
    }

    private void OnEnable()
    {
        _controlsMap.Enable();

        _controlsMap.Controls.ChangeFoot.performed += OnChangeFoot;
        _controlsMap.Controls.Restart.performed += OnRestartButtonPressed;

        AcceptButton = _controlsMap.Controls.Accept.controls[0].name;
        RestartButton = _controlsMap.Controls.Restart.controls[0].name;
        ChangeFootButton = _controlsMap.Controls.ChangeFoot.controls[0].name;
    }
    
    private void OnDisable()
    {
        _controlsMap.Controls.ChangeFoot.performed -= OnChangeFoot;
        _controlsMap.Controls.Restart.performed -= OnRestartButtonPressed;
        
        _controlsMap.Disable();
    }

    private void Update()
    {
        _isHolding = _controlsMap.Controls.Accept.ReadValue<float>().Equals(_holdingValue);
    }
    
    private void OnChangeFoot(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            ChangeKeyPressed?.Invoke();
        }
    }

    private void OnRestartButtonPressed(InputAction.CallbackContext ctx)
    {
        RestartButton = ctx.control.displayName;
        
        if (ctx.performed)
        {
            ResetPressed?.Invoke();
        }
    }
}
