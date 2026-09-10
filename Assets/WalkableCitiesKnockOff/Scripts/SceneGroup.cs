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
        return _scenes.FirstOrDefault(scene => scene.SceneType == sceneType)?.Name;
    }

    public List<SceneData> GetScenes()
    {
        return _scenes.ToList();
    }
}