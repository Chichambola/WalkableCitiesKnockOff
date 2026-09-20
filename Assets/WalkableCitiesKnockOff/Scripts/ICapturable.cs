using UnityEngine;

public interface ICapturable
{
    Vector2 Position { get; }
    Quaternion Rotation { get; }
    bool IsStuck { get; }
    
    
    void ResetToLastState(Vector2 offset);
}