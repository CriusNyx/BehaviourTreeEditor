using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface Queue Stack
/// An interface to makes container data structures ambiguious.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IQStack<T>
{
    T Pop();
    void Push(T t);
    T Peek();
}

public static class IQStackExtensions
{
    public static void PushAll<T>(this IQStack<T> iqStack, IEnumerable<T> enumerable)
    {
        foreach(var e in enumerable)
        {
            iqStack.Push(e);
        }
    }

    public static void PushAll<T>(this IQStack<T> iqStack, IEnumerable enumerable)
    {
        foreach(var e in enumerable)
        {
            iqStack.Push((T)e);
        }
    }

    public static LoopList<T> ToLoopList<T>(this IEnumerable enumerable)
    {
        return new LoopList<T>(enumerable);
    }

    public static WStack<T> ToWStack<T>(this IEnumerable enumerable)
    {
        return new WStack<T>(enumerable);
    }
}
