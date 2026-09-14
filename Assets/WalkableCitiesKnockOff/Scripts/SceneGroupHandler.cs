using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneGroupHandler
{
    public event Action<string> SceneLoaded;
    public event Action<string> SceneUnloaded;
    public event Action Loaded;

    private SceneGroup _activeSceneGroup;
    private List<string> _scenesToKeep;
    private string _coreName = "Core";

    public SceneGroupHandler()
    {
        _scenesToKeep = new List<string>();
    }
    
    public async UniTask RestartActiveScene()
    {
        var name = _activeSceneGroup.FindSceneNameByType(SceneType.ActiveScene);
        
        await SceneManager.UnloadSceneAsync(name);
        
        await SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(name));
    }
    
    public async UniTask LoadScenes(IProgress<float> progress, bool reloadDupScenes = false)
    {
        var loadedScenes = new List<string>();

        int sceneCount = SceneManager.sceneCount;
        
        for (int i = 0; i < sceneCount; i++)
        {
            loadedScenes.Add(SceneManager.GetSceneAt(i).name);
        }

        var totalScenesToLoad = _activeSceneGroup.Count;

        var operationGroup = new AsyncOperationGroup(totalScenesToLoad);

        var sceneData = _activeSceneGroup.GetScenes();
        
        for (int i = 0; i < totalScenesToLoad; i++)
        {
            var scene = sceneData[i];
            
            if (reloadDupScenes == false && loadedScenes.Contains(scene.Name))
                continue;

            var operation = SceneManager.LoadSceneAsync(scene.Path, LoadSceneMode.Additive);

            await UniTask.Delay(TimeSpan.FromSeconds(.5f));
            
            operationGroup.Add(operation);
            
            SceneLoaded?.Invoke(scene.Name);
        }

        while (!operationGroup.IsDone)
        {
            progress?.Report(operationGroup.Progress);

            await UniTask.Delay(TimeSpan.FromSeconds(0.01f));
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
        
        int sceneCount = SceneManager.sceneCount;

        var activeGroupScenes = _activeSceneGroup.GetScenes();

        foreach (var scene in activeGroupScenes.Where(scene => scene.HasSceneType(SceneType.DontDestroy) && !_scenesToKeep.Contains(scene.Name)))
        {
            _scenesToKeep.Add(scene.Name);
        }
        
        for (int i = 0; i < sceneCount; i++)
        {
            bool isFound = false;
            
            var sceneAt = SceneManager.GetSceneAt(i);
            
            if (!sceneAt.isLoaded)
                continue;

            var sceneName = sceneAt.name;

            foreach (var name in _scenesToKeep)
            {
                if (string.Equals(sceneName, name, StringComparison.CurrentCultureIgnoreCase))
                {
                    isFound = true;
                }
            }
            
            if (string.Equals(sceneName, _coreName, StringComparison.CurrentCultureIgnoreCase) || isFound)
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

    public void SetGroup(SceneGroup sceneGroup)
    {
        _activeSceneGroup = sceneGroup;
    }
}