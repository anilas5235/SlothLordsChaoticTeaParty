using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Project.Scripts.DialogScripts.Editor
{
    public class GraphSaveUtility
    {
        private DialogGraphView _targetGraphView;
        private Dialog _dialogCached;
        private List<Edge> Edges => _targetGraphView.edges.ToList();
        private List<DialogNode> Nodes => _targetGraphView.nodes.ToList().Cast<DialogNode>().ToList();

        public static GraphSaveUtility GetInstance(DialogGraphView targetGraphView, Dialog oldData)
        {
            return new GraphSaveUtility()
            {
                _targetGraphView = targetGraphView,
                _dialogCached = oldData,
            };
        }

        public void SaveGraph(string fileName)
        {
            if (!Nodes.Any()) return;//nothing to save

            var dialogContainer = ScriptableObject.CreateInstance<Dialog>();
            
            var nodes = Nodes.ToArray(); 
            
            dialogContainer.passages = new List<DialogPassageNode>();

            if (_dialogCached)
            {
                dialogContainer.dialogCharacter = _dialogCached.dialogCharacter;
                dialogContainer.id = _dialogCached.id;
            }

            for (int i = 0; i < nodes.Length; i++)
            {
                var currentNode = nodes[i];
                dialogContainer.passages.Add(new DialogPassageNode
                {
                    guid = currentNode.guid,
                    text = currentNode.dialogText,
                    rect = currentNode.GetPosition(),
                    entryPoint = currentNode.entryPoint,
                    speaker = currentNode.speaker,
                    audioLine = currentNode.voiceLine,
                    mood = currentNode.mood,
                    imageOverride = currentNode.imageOverride,
                    character = currentNode.character,
                });

                
                List<Edge> outputEdges = new List<Edge>();

                for (int j = 0; j < currentNode.outputContainer.childCount; j++)
                {
                    Port currentElement = currentNode.outputContainer[j].Q<Port>();
                    outputEdges.AddRange(currentElement.connections);
                }

                if (!outputEdges.Any()) continue;

                dialogContainer.passages[i].links = new List<Link>();

                foreach (var edge in outputEdges)
                {
                    if (!(edge.input.node is DialogNode inputNode))continue;

                    dialogContainer.passages[i].links.Add(new Link()
                    {
                        baseNodeGuid = currentNode.guid,
                        portName = edge.output.portName,
                        targetNodeGuid = inputNode.guid,
                    });
                }
            }


            if (!AssetDatabase.IsValidFolder("Assets/Project"))
            {
                AssetDatabase.CreateFolder("Assets", "Project");
            }
            if (!AssetDatabase.IsValidFolder("Assets/Project/Resources"))
            {
                AssetDatabase.CreateFolder("Project", "Resources");
            }
            if (!AssetDatabase.IsValidFolder("Assets/Project/Resources/Dialogs"))
            {
                AssetDatabase.CreateFolder("Resources", "Dialogs");
            }
            
            
            int id = 0;
            string path =$"Assets/Project/Resources/Dialogs/{fileName}.asset";
            while (System.IO.File.Exists(System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), path)))
            {
                path = $"Assets/Project/Resources/Dialogs/{fileName}{id}.asset";
                id++;
            }

            AssetDatabase.CreateAsset(dialogContainer,path);
            AssetDatabase.SaveAssets();
        }

        public void LoadGraph(Dialog dialogData)
        {
            if (!dialogData) { return; }

            _dialogCached = dialogData;
            
            LoadGraph();
        }

        public void LoadGraph(string fileName)
        {
            _dialogCached = Resources.Load<Dialog>($"Dialogs/{fileName}");

            if (_dialogCached == null)
            {
                EditorUtility.DisplayDialog("File Not Found", "The input file path does not exist", "OK");
                return;
            }
            
            LoadGraph();
        }

        private void LoadGraph()
        {
            ClearGraph();
            CreateNodes();
            ConnectNodes();
        }

        private void ConnectNodes()
        {
            for (int i = 0; i < Nodes.Count; i++)
            {
                DialogPassageNode currentPassage = _dialogCached.passages[i];

                if (currentPassage.links != null)
                {
                    if (currentPassage.links.Any())
                    {
                        List<Link> connections = currentPassage.links;

                        for (int j = 0; j < connections.Count; j++)
                        {
                            string targetNodeGuid = connections[j].targetNodeGuid;
                            if (targetNodeGuid == currentPassage.guid|| string.IsNullOrEmpty(targetNodeGuid)) continue;
                            DialogNode targetNode = Nodes.First(x => x.guid == targetNodeGuid);
                            LinkNodes(Nodes[i].outputContainer[j].Q<Port>(), (Port)targetNode.inputContainer[0]);
                        }
                    }
                }

                Nodes[i].SetPosition(_dialogCached.passages[i].rect);

                Nodes[i].RefreshPorts();
                Nodes[i].RefreshExpandedState();
            }
        }

        private void LinkNodes(Port output, Port input)
        {
            var tempEdge = new Edge()
            {
                input = input,
                output = output,
            };
            
            tempEdge.input.Connect(tempEdge);
            tempEdge.output.Connect(tempEdge);
            _targetGraphView.Add(tempEdge);
        }

        private void CreateNodes()
        {
            foreach (var passage in _dialogCached.passages)
            {
                DialogNode tempNode;
                if (passage.entryPoint)
                {
                   tempNode = _targetGraphView.GenerateEntryPoint(passage.rect, passage.guid);
                }
                else
                {
                    tempNode = _targetGraphView.CreateDialogNote(passage.text,passage.speaker,passage.character,passage.audioLine,(int)passage.mood,passage.imageOverride);
                    tempNode.guid = passage.guid;

                    List<Link> nodePorts = passage.links;
                    if (nodePorts != null)
                    {
                        if (nodePorts.Any())
                        {
                            nodePorts.ForEach(x => _targetGraphView.AddChoicePort(tempNode, x.portName));
                        }
                    }
                }

                _targetGraphView.AddElement(tempNode);
            }
        }

        private void ClearGraph()
        {
            foreach (var node in Nodes)
            {
                //Remove connections to node
                Edges.Where(x => x.input.node == node).ToList().ForEach(edge => _targetGraphView.RemoveElement(edge));
                //Remove node
                _targetGraphView.RemoveElement(node);
            }
        }
    }
}
