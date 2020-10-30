using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FlexGroup))]
public class FlexGroupCustomEditor : UnityEditor.Editor
{

    private void OnSceneGUI()
    {
        Handles.BeginGUI();
        if(GUI.Button(new Rect(0,0,150,60),"UPDATE FLEX GROUP"))
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

[CustomEditor(typeof(FlexElement))]
public class FlexElementCustomEditor : UnityEditor.Editor
{

    private void OnSceneGUI()
    {
        Handles.BeginGUI();
        if(GUI.Button(new Rect(0,0,150,60),"UPDATE FLEX GROUP"))
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