using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Project.Scripts.DialogScripts.Editor
{
    public class DialogGraph : EditorWindow
    {
        private DialogGraphView _graphView;
        private string _fileName = "New Narrative";
        private static Dialog _dialogData;
        
        [MenuItem("Graph/Dialog Graph")]
        public static void OpenDialogGraphWindow()
        {
            var window = GetWindow<DialogGraph>();
            window.titleContent = new GUIContent("Dialog Graph");
            _dialogData = null;
        }

        public static void OpenWindow(Dialog dialogData)
        {
            var window = GetWindow<DialogGraph>();
            window.titleContent = new GUIContent("Dialog Graph");
            _dialogData = dialogData;
        }

        private void ConstructGraphView()
        {
            _graphView = new DialogGraphView()
            {
                name = "Dialog Graph"
            };
            
            _graphView.StretchToParentSize();
            rootVisualElement.Add(_graphView);
        }

        private void GenerateToolBar()
        {
            var toolbar = new Toolbar();

            var fileNameTextField = new TextField("File Name:");
            fileNameTextField.SetValueWithoutNotify(_fileName);
            fileNameTextField.MarkDirtyRepaint();
            fileNameTextField.RegisterValueChangedCallback(evt => _fileName = evt.newValue);
            toolbar.Add(fileNameTextField);
            
            toolbar.Add(new Button( () => RequestDataOperation(true)){text = "Save Data"});
            toolbar.Add(new Button(() => RequestDataOperation(false)){text = "Load Data"});

            var createNodeButton = new Button(() =>
            {
                _graphView.CreateNode("New Dialog Node");
            })
            {
                text = "Create Node"
            };

            toolbar.Add(createNodeButton);
            rootVisualElement.Add(toolbar);
        }

        private void LoadData()
        {
            GraphSaveUtility.GetInstance(_graphView).LoadGraph(_dialogData);
        }

        private void RequestDataOperation(bool save)
        {
            if (string.IsNullOrEmpty(_fileName))
            {
                EditorUtility.DisplayDialog("Invalid file name", "Please enter a valid file name","OK");
                return;
            }

            var saveUtility = GraphSaveUtility.GetInstance(_graphView);

            if (save)
            {
                saveUtility.SaveGraph(_fileName);
            }
            else
            {
                saveUtility.LoadGraph(_fileName);
            }
        }

        private void OnEnable()
        {
            ConstructGraphView();
            GenerateToolBar();
            if (_dialogData != null) LoadData();
        }

        private void OnDisable()
        {
            rootVisualElement.Remove(_graphView);
        }
    }
}
