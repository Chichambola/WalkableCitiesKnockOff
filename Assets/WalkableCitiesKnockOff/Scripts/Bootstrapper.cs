using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrapper
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]

    private static async void Init()
    {
        Debug.Log("Core...");

        await SceneManager.LoadSceneAsync("Core", LoadSceneMode.Single);
    }
}
