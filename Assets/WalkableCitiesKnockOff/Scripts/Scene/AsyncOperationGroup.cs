using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public readonly struct AsyncOperationGroup
{
    private readonly List<AsyncOperation> _operations;

    public float Progress => _operations.Count == 0 ? 0 : _operations.Average(o => o.progress);
    public bool IsDone => _operations.All(o => o.isDone);
    
    public AsyncOperationGroup(int initialCapacity)
    {
        _operations = new List<AsyncOperation>(initialCapacity);
    }

    public void Add(AsyncOperation operation)
    {
        _operations.Add(operation);
    }
}