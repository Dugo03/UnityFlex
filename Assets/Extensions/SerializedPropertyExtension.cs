using UnityEditor;
using System;
using System.Linq;
using UnityEngine;

public static class SerializedPropertyExtensions
{
    public static GUIContent[] EnumDescriptions<T>(this SerializedProperty prop)
    {
        return prop.enumDisplayNames.Select(x => new GUIContent(((Enum)Enum.Parse(typeof(T), x, true)).Description())).ToArray();
    }
}