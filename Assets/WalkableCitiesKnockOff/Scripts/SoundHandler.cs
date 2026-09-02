using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundHandler : MonoBehaviour
{
    [SerializeField] private List<AudioClip> _sounds;

    public AudioClip GetSound()
    {
        var index = Random.Range(0, _sounds.Count);
        
        return _sounds[index];
    }
}
