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

    private IPlayer _player;
    
    private void Start()
    {
        _player = Game.GetPlayer();

        _player.SetPosition(_spawnPoint.position);
    }
}
