using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : class
{
    private static Singleton<T> s_instance;

    public static Singleton<T> Instance => s_instance;
    
    protected virtual void Awake()
    {
        if (s_instance != null && s_instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        s_instance = this;
    }
    
    private void OnDestroy()
    {
        if (s_instance == this) 
            s_instance = null;
    }
}
