using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DebugLogger
{
    protected Action<object> action;

    public DebugLogger()
    {
        action = Debug.Log;
    }

    public DebugLogger(Action<object> action)
    {
        this.action = action;
    }

    public virtual void Log(object logable)
    {
        action(logable);
    }
}

public class AILogger : DebugLogger
{
    public List<string> messages = new List<string>();

    public AILogger()
    {
        action = (x) => messages.Add(x.ToString());
    }
}