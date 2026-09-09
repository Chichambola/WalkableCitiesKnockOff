using System;
using System.Collections;
using System.Collections.Generic;
using AYellowpaper;
using TMPro;
using UnityEngine;

public class FirstText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private InterfaceReference<IPlayer, MonoBehaviour> _playerPrefab;

    private IPlayer _player;
    
    private void Awake()
    {
        _player = _playerPrefab.Value;
    }

    private void Start()
    {
        _text.text = $"Press {_player.ChangeFootButton.ToUpper()} to start.";
    }
}
