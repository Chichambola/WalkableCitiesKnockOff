using System;
using System.Collections;
using System.Diagnostics.Contracts;
using System.Linq;
using Eflatun.SceneReference;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

[Serializable]
public class SceneData
{
    [SerializeField] private SceneReference _reference;
    [SerializeField] private SceneType[] _sceneTypes;
    
    public string Name => _reference.Name;
    public string Path => _reference.Path;
    
    public bool HasSceneType(SceneType sceneType) => _sceneTypes.Contains(sceneType);
}