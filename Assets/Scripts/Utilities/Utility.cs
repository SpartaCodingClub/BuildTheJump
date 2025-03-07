using DG.Tweening;
using System;
using UnityEngine;

public class Utility
{
    public static T FindComponent<T>(GameObject gameObject, string name = null) where T : Component
    {
        name ??= typeof(T).Name;
        foreach (var component in gameObject.GetComponentsInChildren<T>(true))
        {
            if (component.name.Equals(name))
            {
                return component;
            }
        }

        Debug.LogWarning($"Failed to FindComponent<{typeof(T).Name}>({gameObject.name}, {name})");
        return null;
    }

    public static string GetTimer(float value)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(value);
        return $"{timeSpan.Minutes:00}:{timeSpan.Seconds:00}";
    }

    public static Sequence RecyclableSequence()
    {
        return DOTween.Sequence().Pause().SetAutoKill(false);
    }
}