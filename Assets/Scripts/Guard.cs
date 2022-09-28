using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

public static class Guard
{
    public static T GetRequiredComponent<T>(this GameObject obj, string paramName) where T : class
    {
        var component = obj.GetComponent<T>();
        
        EditorNotNull(component, paramName, $"A required {typeof(T)} component was not found.");

        return component;
    }

    public static T GetRequiredComponentInChildren<T>(this GameObject obj, string paramName) where T : class
    {
        var component = obj.GetComponentInChildren<T>();
        
        EditorNotNull(component, paramName, $"A required {typeof(T)} component was not found in the children.");

        return component;
    }

    public static void EditorNotNull<T>(T value, string paramName) where T : class =>
        EditorNotNull(value, paramName, $"The {paramName} value cannot be null.");

    public static void EditorNotNull<T>(T value, string paramName, string message) where T : class =>
        ExitPlayModeOnException(() =>
            Assert.IsNotNull(value, $"Guard failed {paramName}: {message}"));

    public static void EditorNotNullOrWhiteSpace(string value, string paramName) =>
        ExitPlayModeOnException(() =>
            Assert.IsTrue(!string.IsNullOrWhiteSpace(value), $"The {paramName} value cannot be null or white space.")
        );

    private static void ExitPlayModeOnException(Action action)
    {
        try
        {
            action();
        }
        catch (Exception e)
        {
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#endif
            throw;
        }
    }
}