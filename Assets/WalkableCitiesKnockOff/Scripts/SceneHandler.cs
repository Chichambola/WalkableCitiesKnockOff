using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    [SerializeField] private Scene _nextScene;

    private Scene _currentScene;

    private void Start()
    {
        _currentScene = SceneManager.GetActiveScene();
    }

    public void Restart()
    {
        SceneManager.LoadScene(_currentScene.name);
    }
}
