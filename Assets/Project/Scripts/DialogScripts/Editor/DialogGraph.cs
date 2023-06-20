using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

/*
 * This System is based on a NoteDialogSystem by Mert Kirimgeri  https://www.youtube.com/@MertKirimgeriGameDev
 */

namespace Project.Scripts.DialogScripts.Editor
{
    public class DialogGraph : EditorWindow
    {
        public static DialogGraph Window { get; protected set; }
        private DialogGraphView graphView;
        private string fileName = "New Narrative";
        private static Dialog _dialogData;
        private TextField fileNameField;
        
        [MenuItem("Graph/Dialog Graph")]
        public static void OpenDialogGraphWindow()
        {
            SetWindow();
            _dialogData = null;
        }

        public void OpenWindow(Dialog dialogData)
        {
            SetWindow();
            _dialogData = dialogData;
            fileName = dialogData.name;
            fileNameField.SetValueWithoutNotify(fileName);
            LoadData();
        }

        private static void SetWindow()
        {
            if (Window) return;

            Window = GetWindow<DialogGraph>();
            Window.titleContent = new GUIContent("Dialog Graph");
        }

        private void ConstructGraphView()
        {
            graphView = new DialogGraphView()
            {
                name = "Dialog Graph"
            };
            
            graphView.StretchToParentSize();
            rootVisualElement.Add(graphView);
        }

        private void GenerateToolBar()
        {
            Toolbar toolbar = new Toolbar();

            fileNameField = new TextField("File Name:");
            fileNameField.SetValueWithoutNotify(fileName);
            fileNameField.MarkDirtyRepaint();
            fileNameField.RegisterValueChangedCallback(evt => fileName = evt.newValue);
            toolbar.Add(fileNameField);
            
            toolbar.Add(new Button( () => RequestDataOperation(true)){text = "Save Data"});
            toolbar.Add(new Button(() => RequestDataOperation(false)){text = "Load Data"});

            Button createNodeButton = new Button(() =>
            {
                graphView.CreateNode("New Dialog Node");
            })
            {
                text = "Create Node"
            };

            toolbar.Add(createNodeButton);
            rootVisualElement.Add(toolbar);
        }

        private void LoadData()
        {
            if(_dialogData) GraphSaveUtility.GetInstance(graphView,_dialogData).LoadGraph(_dialogData);
        }

        private void RequestDataOperation(bool save)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                EditorUtility.DisplayDialog("Invalid file name", "Please enter a valid file name","OK");
                return;
            }

            GraphSaveUtility saveUtility = GraphSaveUtility.GetInstance(graphView, _dialogData);

            if (save) saveUtility.SaveGraph(fileName);
            else saveUtility.LoadGraph(fileName);
        }

        private void OnEnable()
        {
            ConstructGraphView();
            GenerateToolBar();
            LoadData();
        }

        private void OnDisable()
        {
            rootVisualElement.Remove(graphView);
        }
    }
}
