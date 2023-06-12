using System;
using Project.Scripts.UIScripts;
using Project.Scripts.UIScripts.InteractableUI;
using UnityEditor;
using UnityEngine;

namespace Project.Scripts.Editor
{
    [CustomEditor(typeof(StandardButtonFunctions))]
    public class CustomEditorStandardButton : UnityEditor.Editor
    {
        protected SerializedProperty
            P_Function,
            P_WindowHandler,
            P_sceneID;

        protected StandardButtonFunctions  script;

        private void OnEnable()
        {
            script = (StandardButtonFunctions)target;

            P_Function = serializedObject.FindProperty(nameof(script.myFunction));
            P_WindowHandler = serializedObject.FindProperty(nameof(script.menuWindowHandler));
            P_sceneID = serializedObject.FindProperty(nameof(script.sceneID));
        }

        public override void OnInspectorGUI()
        {
            script = (StandardButtonFunctions)target;

            GUILayout.Space(10);

            EditorGUILayout.PropertyField(P_Function);

            switch (script.myFunction)
            {
                case UIMenuWindowHandler.StandardUIButtonFunctions.Esc:
                    break;
                case UIMenuWindowHandler.StandardUIButtonFunctions.ChangeWindow:
                    HasParameters();
                    EditorGUILayout.PropertyField(P_WindowHandler);
                    EditorGUILayout.EndVertical();
                    break;
                case UIMenuWindowHandler.StandardUIButtonFunctions.OpenWindow:
                    HasParameters();
                    EditorGUILayout.PropertyField(P_WindowHandler);
                    EditorGUILayout.EndVertical();
                    break;
                case UIMenuWindowHandler.StandardUIButtonFunctions.Quit:
                    break;
                case UIMenuWindowHandler.StandardUIButtonFunctions.ChangeScene:
                    HasParameters();
                    EditorGUILayout.PropertyField(P_sceneID);
                    EditorGUILayout.EndVertical();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void HasParameters()
        {
            GUILayout.Space(10);
            EditorGUILayout.LabelField("Parameters");
            EditorGUILayout.BeginVertical("Box");
        }
    }
}
