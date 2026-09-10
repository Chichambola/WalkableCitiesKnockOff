using System;

public class LoadingProgress : IProgress<float>
{
    private float _ratio = 1f;
    
    public event Action<float> Progressed;
    
    public void Report(float value)
    {
        Progressed?.Invoke(value / _ratio);
    }
}