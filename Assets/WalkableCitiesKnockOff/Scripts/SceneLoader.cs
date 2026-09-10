using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Image = UnityEngine.UI.Image;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private Image _loadingBar;
    [SerializeField] private float _fillSpeed = 0.5f;
    [SerializeField] private Canvas _loadingCanvas;
    [SerializeField] private Camera _loadingCamera;
    [SerializeField] private SceneGroup[] _sceneGroups;

    private float _targetProgress;
    private bool _isLoading;
    private LoadingProgress _progress;
    private SceneGroupHandler _groupHandler;

    private void Awake()
    {
        _groupHandler = new SceneGroupHandler();
    }

    private void OnEnable()
    {
        _groupHandler.SceneLoaded += OnSceneLoaded;
        _groupHandler.SceneUnloaded += OnSceneUnloaded;
        _groupHandler.Loaded += OnHandlerUnloaded;
    }

    private void Update()
    {
        if (!_isLoading)
            return;

        float currentFillAmount = _loadingBar.fillAmount;
        float progressDifference = Mathf.Abs(currentFillAmount - _targetProgress);

        float dynamicFillSpeed = progressDifference * _fillSpeed;

        _loadingBar.fillAmount = Mathf.Lerp(currentFillAmount, _targetProgress, Time.deltaTime * dynamicFillSpeed);
    }

    private async void Start()
    {
        await LoadSceneGroup(0);
    }

    private void OnDisable()
    {
        _progress.Progressed -= OnLoadingProgressed;
    }

    private async UniTask LoadSceneGroup(int index)
    {
        _loadingBar.fillAmount = 0f;
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
    
    private void OnHandlerUnloaded()
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
