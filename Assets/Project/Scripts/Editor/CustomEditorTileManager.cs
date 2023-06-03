using Project.Scripts.Tiles;
using UnityEditor;
using UnityEngine;

namespace Project.Scripts.Editor
{
    [CustomEditor(typeof(TileFieldManager))]
    public class CustomEditorTileManager : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            TileFieldManager script = (TileFieldManager)target;

            if (GUILayout.Button("Create Grid"))
            {
                script.CreateGrid();
            }
            
            if (GUILayout.Button("NewSave"))
            {
                script.SaveCurrentGrid();
            }
        }
    }
}
