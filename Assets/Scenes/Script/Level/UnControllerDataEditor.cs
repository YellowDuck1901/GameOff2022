using Ink.Runtime;
#if UNITY_EDITOR
using UnityEditor;
#endif
using static UnControllerData;
using UnityEngine;
using System.Security.Cryptography;
using static Mechanic;

#if UNITY_EDITOR
[CustomEditor(typeof(UnControllerData))]
public class UnControllerDataEditor : Editor
{
   
    SerializedProperty _selectedFunction;
    SerializedProperty NumberMovement;
    SerializedProperty LimitTime;
    SerializedProperty isDisable;
    SerializedProperty triggerFunction;
    SerializedProperty Movement;

    private bool attributeGroup = true;
    private void OnEnable()
    {
        _selectedFunction = serializedObject.FindProperty("_selectedFunction");

        NumberMovement = serializedObject.FindProperty("NumberMovement");

        LimitTime = serializedObject.FindProperty("LimitTime");

        isDisable = serializedObject.FindProperty("isDisable");

        triggerFunction = serializedObject.FindProperty("triggerFunction");

        Movement = serializedObject.FindProperty("Movement");
    }

    public override void OnInspectorGUI()
    {

        //Object in inspector
        UnControllerData UnControllerData = (UnControllerData)target;

        serializedObject.Update();

        EditorGUILayout.PropertyField(_selectedFunction);
        EditorGUILayout.PropertyField(triggerFunction);

        attributeGroup = EditorGUILayout.BeginFoldoutHeaderGroup(attributeGroup, "Attribute Group");

        if (attributeGroup)
        {
            EditorGUILayout.PropertyField(Movement);
            switch (UnControllerData._selectedFunction)
            {
                case UnControllerData.FunctionOption.limitNumberMovement:
                    EditorGUILayout.PropertyField(NumberMovement);
                    break;
                case UnControllerData.FunctionOption.limitTimePressMovement:
                    EditorGUILayout.PropertyField(LimitTime);
                    break;
                case UnControllerData.FunctionOption.doActionInTimeRange:
                    EditorGUILayout.PropertyField(NumberMovement);
                    EditorGUILayout.PropertyField(LimitTime);
                    break;
                case UnControllerData.FunctionOption.setDisableMovement:
                    EditorGUILayout.PropertyField(isDisable);
                    break;
                case UnControllerData.FunctionOption.limitAndDisableNumberMovement:
                    EditorGUILayout.PropertyField(NumberMovement);
                    break;
            }
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
     

        serializedObject.ApplyModifiedProperties();
    }
}
#endif
