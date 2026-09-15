using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private EndOfLevel _endOfLevel;

    public event Action<IPlayer> Loaded;
    
    private IPlayer _player;

    private void Start()
    {
        InitializePlayer();
        
        EndOfLevelHandler.Init(_endOfLevel, EndOfLevelHandler.Instance as EndOfLevelHandler);
    }

    private void InitializePlayer()
    {
        _player = Game.GetPlayer();
        
        _player.Set(_spawnPoint.position);

        _player.Set(_spawnPoint.rotation);
        
        Loaded?.Invoke(_player);
    }
}
