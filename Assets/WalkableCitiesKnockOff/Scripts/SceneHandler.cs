using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json.Bson;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private SceneReadyChannel _channel;
    [SerializeField] private EndOfLevel _endOfLevel;
    
    private IPlayer _player;

    private void OnEnable()
    {
        _channel.Raised += InitializePlayer;
    }

    private void OnDisable()
    {
        _channel.Raised -= InitializePlayer;
    }

    private void Start()
    {
        EndOfLevelHandler.Init(_endOfLevel, EndOfLevelHandler.Instance as EndOfLevelHandler);
    }

    private void InitializePlayer(IPlayer player)
    {
        _player = player;
        
        _player.Set(_spawnPoint.position);

        _player.Set(_spawnPoint.rotation);
    }
}
