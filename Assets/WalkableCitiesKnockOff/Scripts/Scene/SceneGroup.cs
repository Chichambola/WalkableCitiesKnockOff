using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class SceneGroup
{
    [SerializeField] private string _groupName = "New group name";
    [SerializeField] private List<SceneData> _scenes;

    public int Count => _scenes.Count;
    
    public string FindSceneNameByType(SceneType sceneType)
    {
        foreach (var scene in _scenes.Where(scene => scene.HasSceneType(sceneType)))
        {
            return scene.Name;
        }

        throw new KeyNotFoundException($"There are no scene with type {sceneType}");
    }

    public List<SceneData> GetScenes()
    {
        return _scenes.ToList();
    }
}