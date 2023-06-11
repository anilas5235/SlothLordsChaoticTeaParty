using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace Project.Scripts.DialogScripts.Editor
{
    public abstract class AssetHandler
    {
        [OnOpenAsset()]
        public static bool OpenEditor(int instanceID, int line)
        {
            Dialog obj = EditorUtility.InstanceIDToObject(instanceID) as Dialog;
            if (obj != null)
            {
                DialogGraph.OpenWindow(obj);
                return true;
            }
            return false;
        }
    }
    [CustomEditor(typeof(Dialog))]
    public class CustomEditorDialog : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Open Graph"))
            { OpenGraph(); }

            
            base.OnInspectorGUI();
        }

        private void OpenGraph()
        {
            DialogGraph.OpenWindow((Dialog)target);
        }
    }
}
