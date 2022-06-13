using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(VariableLanguage)), CanEditMultipleObjects]
public class VariableLanguageEditor : Editor
{

    public SerializedProperty
        dataType,
        category,
        rus,
        eng,
        strRus,
        strEng;

    void OnEnable()
    {
        // Setup the SerializedProperties
        dataType = serializedObject.FindProperty("dataType");
        category = serializedObject.FindProperty("category");
        rus = serializedObject.FindProperty("rus");
        eng = serializedObject.FindProperty("eng");
        strRus = serializedObject.FindProperty("strRus");
        strEng = serializedObject.FindProperty("strEng");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(dataType);

        VariableLanguage.DataType st = (VariableLanguage.DataType)dataType.enumValueIndex;

        switch (st)
        {
            case VariableLanguage.DataType.Image:
                EditorGUILayout.PropertyField(rus, new GUIContent("rus"));
                EditorGUILayout.PropertyField(eng, new GUIContent("eng"));
                break;

            case VariableLanguage.DataType.Text:
                EditorGUILayout.PropertyField(strRus, new GUIContent("strRus"));
                EditorGUILayout.PropertyField(strEng, new GUIContent("strEng"));
                break;

            case VariableLanguage.DataType.Int:
                EditorGUILayout.PropertyField(category, new GUIContent("category"));
                break;

        }


        serializedObject.ApplyModifiedProperties();
    }
}
