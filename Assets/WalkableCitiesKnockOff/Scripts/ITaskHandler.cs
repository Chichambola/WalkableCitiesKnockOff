using System;

public interface ITaskHandler
{
    public event Action Completed;
}