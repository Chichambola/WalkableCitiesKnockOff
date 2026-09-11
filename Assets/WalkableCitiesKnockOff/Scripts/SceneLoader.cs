using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class SceneLoader : Singleton<SceneLoader>
{
    [SerializeField] private Slider _loadingBar;
    [SerializeField] private float _fillSpeed = 0.5f;
    [SerializeField] private Canvas _loadingCanvas;
    [SerializeField] private Camera _loadingCamera;
    [SerializeField] private SceneGroup[] _sceneGroups;

    private float _targetProgress;
    private int _currentSceneIndex;
    private bool _isLoading;
    private LoadingProgress _progress;
    private SceneGroupHandler _groupHandler;

    private void Awake()
    {
        _groupHandler = new SceneGroupHandler();
    }

    private void Update()
    {
        if (!_isLoading)
            return;

        float currentFillAmount = _loadingBar.value;
        float progressDifference = Mathf.Abs(currentFillAmount - _targetProgress);

        float dynamicFillSpeed = progressDifference * _fillSpeed;
        
        _loadingBar.value = Mathf.MoveTowards(currentFillAmount, _targetProgress, Time.deltaTime * dynamicFillSpeed);
    }

    public void RestartActiveScene()
    {
        _groupHandler.RestartActiveScene().Forget();
    }
    
    private async void Start()
    {
        await LoadSceneGroup(0);
    }

    private void OnEnable()
    {
        Game.RequestedRestart += RestartActiveScene;
        _groupHandler.SceneLoaded += OnSceneLoaded;
        _groupHandler.SceneUnloaded += OnSceneUnloaded;
        _groupHandler.Loaded += OnHandlerLoaded;
    }
    
    private void OnDisable()
    {
        Game.RequestedRestart -= RestartActiveScene;
        _progress.Progressed -= OnLoadingProgressed;
        _groupHandler.SceneLoaded -= OnSceneLoaded;
        _groupHandler.SceneUnloaded -= OnSceneUnloaded;
        _groupHandler.Loaded -= OnHandlerLoaded;
    }
    
    private async UniTask LoadSceneGroup(int index)
    {
        _currentSceneIndex = index;
        
        _loadingBar.value = 0f;
        
        _targetProgress = 1f;

        if (index < 0 || index >= _sceneGroups.Length)
        {
            Debug.LogError($"Invalid scene group index : {index}");
            
            return;
        }

        _progress = new LoadingProgress();

        _progress.Progressed += OnLoadingProgressed;
        
        EnableLoadingCanvas();

        await _groupHandler.LoadScenes(_sceneGroups[index], _progress);
        
        EnableLoadingCanvas(false);
    }

    private void OnLoadingProgressed(float value)
    {
        _targetProgress = Mathf.Max(value, _targetProgress);
    }

    private void EnableLoadingCanvas(bool enable = true)
    {
        _isLoading = true;
        _loadingCanvas.gameObject.SetActive(enable);
        _loadingCamera.gameObject.SetActive(enable);
    }
    
    private void OnHandlerLoaded()
    {
        Debug.Log("Scene group loaded");
    }

    private void OnSceneUnloaded(string obj)
    {
        Debug.Log($"Unloaded: {obj}");
    }

    private void OnSceneLoaded(string obj)
    {
        Debug.Log($"Loaded: {obj}");
        
        
    }
}
