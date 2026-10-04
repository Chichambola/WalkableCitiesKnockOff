using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/Scene Ready Channel")]
public class SceneReadyChannel : ScriptableObject
{
    public event Action<IPlayer> Raised;

    public void Raise(IPlayer player)
    {
        Raised?.Invoke(player);
    }
}