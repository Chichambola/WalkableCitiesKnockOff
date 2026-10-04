using System;

public interface IPoolable<T>
{
    public event Action<T> CanBeReleased;
    void Release();
}