using Project.Scripts.Tiles;
using UnityEditor;
using UnityEngine;

namespace Project.Scripts.Editor
{
    [CustomEditor(typeof(TileManager))]
    public class CustomEditorTileManager : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            TileManager script = (TileManager)target;

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
