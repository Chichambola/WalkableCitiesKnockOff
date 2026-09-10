using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Cysharp.Threading.Tasks;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneGroupHandler
{
    public event Action<string> SceneLoaded;
    public event Action<string> SceneUnloaded;
    public event Action Loaded;

    private SceneGroup _activeSceneGroup;
    private string _coreName = "Core";

    public async UniTask LoadScenes(SceneGroup group, IProgress<float> progress, bool reloadDupScenes = false)
    {
        _activeSceneGroup = group;

        var loadedScenes = new List<string>();
        
        await UnloadScenes();

        int sceneCount = SceneManager.sceneCount;

        for (int i = 0; i < sceneCount; i++)
        {
            loadedScenes.Add(SceneManager.GetSceneAt(i).name);
        }

        var totalScenesToLoad = _activeSceneGroup.Count;

        var operationGroup = new AsyncOperationGroup(totalScenesToLoad);

        var sceneData = group.GetScenes();
        
        for (int i = 0; i < totalScenesToLoad; i++)
        {
            var scene = sceneData[i];

            if (reloadDupScenes == false && loadedScenes.Contains(scene.Name))
                continue;

            var operation = SceneManager.LoadSceneAsync(scene.Path, LoadSceneMode.Additive);
            
            operationGroup.Add(operation);
            
            SceneLoaded?.Invoke(scene.Name);
        }

        while (!operationGroup.IsDone)
        {
            progress?.Report(operationGroup.Progress);

            await UniTask.Delay(TimeSpan.FromSeconds(1));
        }

        Scene activeScene = SceneManager.GetSceneByName(_activeSceneGroup.FindSceneNameByType(SceneType.ActiveScene));

        if (activeScene.IsValid())
        {
            SceneManager.SetActiveScene(activeScene);
        }
        
        Loaded?.Invoke();
    }

    public async UniTask UnloadScenes()
    {
        var scenes = new List<string>();
        var activeScene = SceneManager.GetActiveScene().name;

        int sceneCount = SceneManager.sceneCount;

        for (int i = 0; i < sceneCount; i++)
        {
            var sceneAt = SceneManager.GetSceneAt(i);

            if (!sceneAt.isLoaded)
                continue;

            var sceneName = sceneAt.name;
            
            if (sceneName.Equals(activeScene) || string.Equals(sceneName, _coreName, StringComparison.CurrentCultureIgnoreCase))
                continue;
            
            scenes.Add(sceneName);
        }

        var operationGroup = new AsyncOperationGroup(scenes.Count);

        foreach (var scene in scenes)
        {
            var operation = SceneManager.UnloadSceneAsync(scene);

            if (operation == null)
                continue;
            
            operationGroup.Add(operation);
            
            SceneUnloaded?.Invoke(scene);
        }
        
        while (!operationGroup.IsDone)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(1));
        }
    }
}