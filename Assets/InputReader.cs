using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class InputReader : MonoBehaviour
{
    private ControlsMap _controlsMap;

    public event Action ChangeKeyPressed;
    public event Action ResetPressed;

    private void Awake()
    {
        _controlsMap = new ControlsMap();
    }

    private void OnEnable()
    {
        _controlsMap.Enable();
    }

    private void OnDestroy()
    {
        _controlsMap.Disable();
    }

    private void Update()
    {
        if (_controlsMap.Controls.ChangeFoot.WasPressedThisFrame())
            ChangeKeyPressed?.Invoke();

        if (_controlsMap.Controls.Restart.WasPressedThisFrame())
            ResetPressed?.Invoke();
    }
}
