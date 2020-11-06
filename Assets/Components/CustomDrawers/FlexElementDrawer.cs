using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FlexElement))]
public class FlexElementCustomEditor : UnityEditor.Editor
{
    private SerializedProperty Flex { get; set; }
    private SerializedProperty MinWidthType { get; set; }
    private SerializedProperty MinWidthUnit { get; set; }
    private SerializedProperty MinWidthPercent { get; set; }
    private SerializedProperty MinHeightType { get; set; }
    private SerializedProperty MinHeightUnit { get; set; }
    private SerializedProperty MinHeightPercent { get; set; }

    public static void DrawUnitTypeToogle(SerializedProperty property)
    {
        var position = EditorGUILayout.GetControlRect(hasLabel:true);
        var propertyLabel = EditorGUI.BeginProperty(position, null, property);
        var insideRect = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), propertyLabel);

        var buttonsText = new GUIContent[]{
            new GUIContent("%"),
            new GUIContent("px"),
        };

        property.enumDisplayNames.Select(x => new GUIContent(x)).ToArray();

        property.enumValueIndex = GUI.Toolbar(insideRect, property.enumValueIndex, buttonsText, EditorStyles.miniButton, GUI.ToolbarButtonSize.Fixed);
        EditorGUI.EndProperty();
    }

    void OnEnable()
    {
        Flex = serializedObject.FindProperty("_flex");

        MinWidthType = serializedObject.FindProperty("_minWidthType");
        MinWidthUnit = serializedObject.FindProperty("_minWidthUnit");
        MinWidthPercent = serializedObject.FindProperty("_minWidthPercent");

        MinHeightType = serializedObject.FindProperty("_minHeightType");
        MinHeightUnit = serializedObject.FindProperty("_minHeightUnit");
        MinHeightPercent = serializedObject.FindProperty("_minHeightPercent");
    }

    private void OnSceneGUI()
    {
        Handles.BeginGUI();
        if(GUI.Button(new Rect(0,0,150,60),"UPDATE FLEX GROUP"))
        {
            ForceUpdateFlex();
        }
        Handles.EndGUI();
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.PropertyField(Flex);

        EditorGUILayout.Space();

        DrawUnitTypeToogle(MinWidthType);
        if (MinWidthType.enumValueIndex == (int)UnitType.percent)
            EditorGUILayout.PropertyField(MinWidthPercent, new GUIContent("Min Width"));
        else
            EditorGUILayout.PropertyField(MinWidthUnit, new GUIContent("Min Width"));
        serializedObject.ApplyModifiedProperties();

        EditorGUILayout.Space();

        DrawUnitTypeToogle(MinHeightType);
        if (MinHeightType.enumValueIndex == (int)UnitType.percent)
            EditorGUILayout.PropertyField(MinHeightPercent, new GUIContent("Min Height"));
        else
            EditorGUILayout.PropertyField(MinHeightUnit, new GUIContent("Min Height"));
        serializedObject.ApplyModifiedProperties();
    }

    private void ForceUpdateFlex()
    {
        var flexGroups = Resources.FindObjectsOfTypeAll<FlexGroup>();
        foreach (var flexGroup in flexGroups)
        {
            flexGroup.Init();
        }
    }
}