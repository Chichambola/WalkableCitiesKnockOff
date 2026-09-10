using System;
using System.Collections;
using Eflatun.SceneReference;
using UnityEngine;

[Serializable]
public class SceneData
{
    [SerializeField] private SceneReference _reference;
    [SerializeField] private SceneType _sceneType;
    
    public string Name => _reference.Name;
    public SceneType SceneType => _sceneType;
    public string Path => _reference.Path;
}