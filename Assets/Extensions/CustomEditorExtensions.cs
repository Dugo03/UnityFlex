using UnityEditor;
using UnityEngine;

public static class CustomEditorExtensions
{
    public static void DrawEnumAsButtonToggle<T>(this Editor editor, SerializedProperty property)
    {
        var position = EditorGUILayout.GetControlRect(hasLabel: true);
        var propertyLabel = EditorGUI.BeginProperty(position, null, property);
        var insideRect = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), propertyLabel);

        property.enumValueIndex = GUI.Toolbar(insideRect, property.enumValueIndex, property.EnumDescriptions<T>(), EditorStyles.miniButton, GUI.ToolbarButtonSize.Fixed);
        EditorGUI.EndProperty();
    }
}