using UnityEngine;
using UnityEditor;
using System.Linq;

[CustomEditor(typeof(FlexGroup))]
public class FlexGroupCustomEditor : UnityEditor.Editor
{
    private SerializedProperty FlexDirection { get; set; }
    private SerializedProperty PaddingHorizontalType { get; set; }
    private SerializedProperty PaddingHorizontalPercent { get; set; }
    private SerializedProperty PaddingHorizontalUnit { get; set; }
    private SerializedProperty PaddingVerticalType { get; set; }
    private SerializedProperty PaddingVerticalPercent { get; set; }
    private SerializedProperty PaddingVerticalUnit { get; set; }
    private SerializedProperty ItemAlignment { get; set; }

    void OnEnable()
    {
        FlexDirection = serializedObject.FindProperty("_flexDirection");

        PaddingHorizontalType = serializedObject.FindProperty("_paddingHorizontalType");
        PaddingHorizontalPercent = serializedObject.FindProperty("_paddingHorizontalPercent");
        PaddingHorizontalUnit = serializedObject.FindProperty("_paddingHorizontalUnit");

        PaddingVerticalType = serializedObject.FindProperty("_paddingVerticalType");
        PaddingVerticalPercent = serializedObject.FindProperty("_paddingVerticalPercent");
        PaddingVerticalUnit = serializedObject.FindProperty("_paddingVerticalUnit");
        ItemAlignment = serializedObject.FindProperty("_itemAlignment");
    }

    public override void OnInspectorGUI()
    {
        this.DrawEnumAsButtonToggle<FlexDirection>(FlexDirection);
        this.DrawEnumAsButtonToggle<AlignmentType>(ItemAlignment);

        EditorGUILayout.Space();

        this.DrawEnumAsButtonToggle<UnitType>(PaddingHorizontalType);
        if (PaddingHorizontalType.enumValueIndex == (int)UnitType.percent)
            EditorGUILayout.PropertyField(PaddingHorizontalPercent, new GUIContent("Horizontal Padding"));
        else
            EditorGUILayout.PropertyField(PaddingHorizontalUnit, new GUIContent("Horizontal Padding"));
        serializedObject.ApplyModifiedProperties();

        EditorGUILayout.Space();

        this.DrawEnumAsButtonToggle<UnitType>(PaddingVerticalType);
        if (PaddingVerticalType.enumValueIndex == (int)UnitType.percent)
            EditorGUILayout.PropertyField(PaddingVerticalPercent, new GUIContent("Vertical Padding"));
        else
            EditorGUILayout.PropertyField(PaddingVerticalUnit, new GUIContent("Vertical Padding"));
        serializedObject.ApplyModifiedProperties();
    }

    private void OnSceneGUI()
    {
        Handles.BeginGUI();
        if (GUI.Button(new Rect(0, 0, 150, 60), "UPDATE FLEX GROUP"))
        {
            ForceUpdateFlex();
        }
        Handles.EndGUI();
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