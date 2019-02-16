using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ambiguous queue and automatically enqueues removed entries.
/// </summary>
public class LoopList<T> : IQStack<T>
{
    private Queue<T> queue = new Queue<T>();

    public LoopList()
    {

    }

    public LoopList(IEnumerable enumerable)
    {
        this.PushAll(enumerable);
    }

    public T Peek()
    {
        return queue.Peek();
    }

    public T Pop()
    {
        T t = queue.Dequeue();
        queue.Enqueue(t);
        return t;
    }

    public void Push(T t)
    {
        queue.Enqueue(t);
    }

    public T PopRemove()
    {
        return queue.Dequeue();
    }
}