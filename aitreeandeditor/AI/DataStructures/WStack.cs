using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Wrapped Stack
/// A wrapper class for an ambiguous stack.
/// </summary>
public class WStack<T> : IQStack<T>
{
    private Stack<T> stack = new Stack<T>();

    public WStack()
    {

    }

    public WStack(IEnumerable enumerable)
    {
        this.PushAll(enumerable);
    }

    public T Peek()
    {
        return stack.Peek();
    }

    public T Pop()
    {
        return stack.Pop();
    }

    public void Push(T t)
    {
        stack.Push(t);
    }
}