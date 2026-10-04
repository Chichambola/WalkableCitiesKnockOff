using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/Requested sound channel")]
public class SoundRequestChannel : ScriptableObject
{
    public event Action<ESoundType, Vector2> Raised;

    public void Raise(ESoundType type, Vector2 position)
    {
        Raised?.Invoke(type, position);
    }
}