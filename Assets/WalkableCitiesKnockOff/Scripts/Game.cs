using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    [SerializeField] private Player _player;

    private void OnEnable()
    {
        _player.RequestedRestart += Restart;
    }

    private void OnDisable()
    {
        _player.RequestedRestart -= Restart;
    }

    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
