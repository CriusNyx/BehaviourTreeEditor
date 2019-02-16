using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Wrapped Queue
/// A wrapper class for an ambiguous queue
/// </summary>
public class WQueue<T> : IQStack<T>
{
    private Queue<T> queue = new Queue<T>();

    public WQueue()
    {

    }

    public WQueue(IEnumerable enumerable)
    {
        this.PushAll(enumerable);
    }

    public T Peek()
    {
        return queue.Peek();
    }

    public T Pop()
    {
        return queue.Dequeue();
    }

    public void Push(T t)
    {
        queue.Enqueue(t);
    }
}