using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    [SerializeField] private EndOfLevelHandler _endOfLevelHandler;
    [SerializeField] private Transform _spawnPoint;

    public event Action<IPlayer> Loaded;
    
    private IPlayer _player;

    private void OnEnable()
    {
        _endOfLevelHandler.RequestedEndOfLevel += OnRequestedEndOfLevel;
    }
    
    private void OnDisable()
    {
        _endOfLevelHandler.RequestedEndOfLevel -= OnRequestedEndOfLevel;
    }

    private void Start()
    {
        _player = Game.GetPlayer();
        
        Loaded?.Invoke(_player);

        _player.SetPosition(_spawnPoint.position);
    }
    
    private void OnRequestedEndOfLevel()
    {
        Game.LoadNextLevel();
    }
}
