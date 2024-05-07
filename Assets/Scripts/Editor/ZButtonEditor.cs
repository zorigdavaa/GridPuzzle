using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ZButton))]
public class ZButtonEditor : UnityEditor.UI.ButtonEditor
{
    private SerializedProperty onDownEvent;
    private SerializedProperty stateChanged;
    private void OnEnable()
    {
        base.OnEnable();
        onDownEvent = serializedObject.FindProperty("m_OnDown");
        stateChanged = serializedObject.FindProperty("buttonStateChanged");
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.Space();

        serializedObject.Update();
        EditorGUILayout.PropertyField(onDownEvent);
        EditorGUILayout.PropertyField(stateChanged);
        serializedObject.ApplyModifiedProperties();
    }
}
