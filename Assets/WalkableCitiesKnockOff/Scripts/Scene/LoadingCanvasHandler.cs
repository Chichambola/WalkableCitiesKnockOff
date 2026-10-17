using Cysharp.Threading.Tasks;
using PrimeTween;
using UnityEngine;

public class LoadingCanvasHandler : MonoBehaviour
{
    [SerializeField] private Canvas _loadingCanvas;
    [SerializeField] private Camera _loadingCamera;
    [SerializeField] private float _fadeDuration = 1f;
    [SerializeField] private CanvasGroup _background;

    private TweenSettings<float> _settings;
    
    private void Awake()
    {
        _settings = new TweenSettings<float>();

        _settings.settings.duration = _fadeDuration;
        _settings.settings.useUnscaledTime = true;
    }

    public void SetActive(bool value)
    {
        _loadingCanvas.gameObject.SetActive(value);
        _loadingCamera.gameObject.SetActive(value);
    }
    
    public async UniTask ChangeOpacity(float targetValue)
    {
        _settings.startValue = _background.alpha;
        _settings.endValue = targetValue;
            
        await Tween.Alpha(_background, _settings);
    }
}