using UnityEngine;

public interface ICapturable
{
    Vector2 Position { get; }
    Quaternion Rotation { get; }

    void ResetToLastState();
}