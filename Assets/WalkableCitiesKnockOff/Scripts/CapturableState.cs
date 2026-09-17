using UnityEngine;

public struct CapturableState
{
    public Vector2 Position { get; private set; }
    public Quaternion Rotation { get; private set; }
    
    public CapturableState(Vector2 position, Quaternion rotation)
    {
        Position = position;
        Rotation = rotation;
    }
}