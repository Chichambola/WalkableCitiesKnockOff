using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Eflatun.SceneReference;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrapper
{
    private static List<string> s_scenes;
    private const string Core = nameof(Core);
    private const string Gameplay = nameof(Gameplay);
    private const string UI = nameof(UI);
    
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static async void Init()
    {
        await SceneManager.LoadSceneAsync(Core, LoadSceneMode.Single);
    }
}
