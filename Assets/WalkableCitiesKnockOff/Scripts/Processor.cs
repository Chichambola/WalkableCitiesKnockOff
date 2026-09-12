using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Processor<T> : MonoBehaviour where T : class
{
    public abstract void Process(T @object);
}
