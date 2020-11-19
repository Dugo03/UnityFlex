using System;
using System.ComponentModel;
using System.Reflection;
using UnityEditor;

public static class EnumExtensions
{
    public static string Description(this Enum e)
    {
        DescriptionAttribute[] attributes = (DescriptionAttribute[])e
               .GetType()
               .GetField(e.ToString())
               .GetCustomAttributes(typeof(DescriptionAttribute), false);
        return attributes.Length > 0 ? attributes[0].Description : string.Empty;
    }
}