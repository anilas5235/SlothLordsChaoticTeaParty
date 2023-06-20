using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Project.Scripts.DialogScripts.Editor
{
    public class DialogGraphView : GraphView
    {
        private readonly Vector2 defaultNodeSize = new Vector2(150, 200);
        private readonly float defaultLabelWidth = 70f;

        public DialogGraphView(Dialog dialog = null)
        {
            styleSheets.Add(Resources.Load<StyleSheet>("DialogGraph"));
            SetupZoom(ContentZoomer.DefaultMinScale,ContentZoomer.DefaultMaxScale);
            
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            var grid = new GridBackground();         
            Insert(0,grid);
            grid.StretchToParentSize();
            if (dialog != null)
            {
                GraphSaveUtility.GetInstance(this,dialog).LoadGraph(dialog);
            }
            AddElement(GenerateEntryPoint(new Vector2(150,200)));
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            var compatiblePorts = new List<Port>();

            ports.ForEach((port) =>
            {
                if (startPort!= port && startPort.node != port.node)
                {
                    compatiblePorts.Add(port);
                }
            });

            return compatiblePorts;
        }

        private DialogNode GenerateEntryPoint( Vector2 position,string guid = "")
        {
            var node = new DialogNode()
            {
                title = "START",
                guid = string.IsNullOrEmpty(guid) ? Guid.NewGuid().ToString():guid,
                dialogText = "ENTRYPOINT",
                entryPoint = true,
            };

            var port =  GeneratePort(node, Direction.Output);
            port.portName = "Next";
            node.outputContainer.Add(port);
            
            node.RefreshPorts();
            node.RefreshExpandedState();
            
            node.SetPosition(new Rect(position, new Vector2(100,150)));

            return node;
        }
        
        public DialogNode GenerateEntryPoint( Rect position,string guid = "")
        {
            var node = new DialogNode()
            {
                title = "START",
                guid = string.IsNullOrEmpty(guid) ? Guid.NewGuid().ToString():guid,
                dialogText = "ENTRYPOINT",
                entryPoint = true,
            };

            var port =  GeneratePort(node, Direction.Output);
            port.portName = "Next";
            node.outputContainer.Add(port);
            
            node.RefreshPorts();
            node.RefreshExpandedState();
            
            node.SetPosition(position);

            return node;
        }

        private Port GeneratePort(DialogNode targetNode, Direction portDirection, Port.Capacity portCapacity = Port.Capacity.Single)
        {
            return targetNode.InstantiatePort(Orientation.Horizontal, portDirection, portCapacity, typeof(float)); // type not used
        }

        public void CreateNode(string nodeName)
        {
            AddElement(CreateDialogNote(nodeName));
        }


        public DialogNode CreateDialogNote(string nodeName, string speakerName ="speaker", 
            CharacterAnimator.Characters dataCharacter = CharacterAnimator.Characters.None , AudioClip audioClip = null,
           int characterMoodIndex = 0, Sprite overrideImage = null)
        {
            var newNode = new DialogNode
            {
                dialogText = nodeName,
                title = (nodeName.Length < 20) ? nodeName : nodeName.Substring(0,20),
                guid = Guid.NewGuid().ToString(),
                speaker = speakerName,
                voiceLine = audioClip,
                mood = (CharacterAnimator.CharacterMoods) characterMoodIndex,
                imageOverride = overrideImage,
                character = dataCharacter,
            };

            var inputPort = GeneratePort(newNode, Direction.Input, Port.Capacity.Multi);
            inputPort.portName = "Input";
            
            newNode.inputContainer.Add(inputPort);

            var createOutputButton = new Button(() =>
            {
                AddChoicePort(newNode);
            })
            {
                text = "New Choice"
            };
            
            newNode.titleContainer.Add(createOutputButton);
            
            
            newNode.mainContainer.Add(new Label("   "));
            
            
            TextField speakerTextField = new TextField("Speaker:");
            speakerTextField.SetValueWithoutNotify(newNode.speaker);
            speakerTextField.RegisterValueChangedCallback(evt =>
            {
                newNode.speaker = evt.newValue;
            });
            speakerTextField.labelElement.style.minWidth = defaultLabelWidth;
            
            
            EnumField characterField = new EnumField("Character:", CharacterAnimator.Characters.None);
            characterField.RegisterValueChangedCallback(evt =>
            {
                newNode.character = (CharacterAnimator.Characters)evt.newValue;
                newNode.speaker = newNode.character.ToString();
                speakerTextField.SetValueWithoutNotify(newNode.speaker);
            });
            characterField.SetValueWithoutNotify(newNode.character);
            characterField.labelElement.style.minWidth = defaultLabelWidth;
            newNode.mainContainer.Add(characterField);
            
            
            newNode.mainContainer.Add(speakerTextField);
            
            newNode.mainContainer.Add(new Label("   "));

            TextField dialogText = new TextField(string.Empty);
            dialogText.RegisterValueChangedCallback(evt =>
            {
                newNode.dialogText = evt.newValue;
                newNode.title = (evt.newValue.Length < 20) ? evt.newValue : evt.newValue.Substring(0,20);
            });
            dialogText.style.minHeight = 40f;
            dialogText.style.maxWidth = 300f;
            dialogText.SetValueWithoutNotify(nodeName);
            newNode.mainContainer.Add(dialogText);
            
            newNode.mainContainer.Add(new Label("   "));

            ObjectField audioField = new ObjectField("Voice Line")
            {
                objectType = typeof(AudioClip)
            };
            audioField.RegisterValueChangedCallback(evt =>
            {
                if (evt.newValue is AudioClip clip)
                {
                   newNode.voiceLine = clip; 
                }
            });
            audioField.SetValueWithoutNotify(newNode.voiceLine);
            audioField.labelElement.style.minWidth = defaultLabelWidth;
            newNode.mainContainer.Add(audioField);

            EnumField moodField = new EnumField("Mood:", CharacterAnimator.CharacterMoods.Neutral);
            moodField.RegisterValueChangedCallback(evt =>
            {
                newNode.mood = (CharacterAnimator.CharacterMoods)evt.newValue;
            });
            moodField.SetValueWithoutNotify(newNode.mood);
            moodField.labelElement.style.minWidth = defaultLabelWidth;
            newNode.mainContainer.Add(moodField);

            ObjectField imageOverrideField = new ObjectField("OverrideImage:")
            {
                objectType = typeof(Sprite),
            };
            imageOverrideField.RegisterValueChangedCallback(evt =>
            {
                if (evt.newValue is Sprite sprite)
                {
                    newNode.imageOverride = sprite; 
                }
            } );
            imageOverrideField.labelElement.style.minWidth = defaultLabelWidth;
            imageOverrideField.SetValueWithoutNotify(newNode.imageOverride);
            newNode.mainContainer.Add(imageOverrideField);

            newNode.RefreshPorts();
            newNode.RefreshExpandedState();
            
            newNode.SetPosition(new Rect(Vector2.zero, defaultNodeSize));

            return newNode;
        }

        public void AddChoicePort(DialogNode targetNode, string overridenPortName = "")
        {
            var generatedPort = GeneratePort(targetNode, Direction.Output);

            var oldLabel = generatedPort.contentContainer.Q<Label>("type");
            generatedPort.contentContainer.Remove(oldLabel);

            int outputCount = targetNode.outputContainer.Query("connector").ToList().Count;

            var choicePortName = string.IsNullOrEmpty(overridenPortName) ? $"Choice {outputCount}" : overridenPortName;
            
            generatedPort.portName = choicePortName;
            var textField = new TextField
            {
                name = string.Empty,
                value = choicePortName,
                style =
                {
                    minWidth = defaultLabelWidth,
                    maxWidth = 150f,
                }
            };
            textField.RegisterValueChangedCallback(evt => generatedPort.portName = evt.newValue);
            
            generatedPort.contentContainer.Add(new Label("   "));
            generatedPort.contentContainer.Add(textField);

            var deleteButton = new Button(() =>
            {
                RemovePort(targetNode, generatedPort);
            })
            {
                text = "X"
            };
            generatedPort.contentContainer.Add(deleteButton);
            
            targetNode.outputContainer.Add(generatedPort);
            targetNode.RefreshPorts();
            targetNode.RefreshExpandedState();
        }

        private void RemovePort(DialogNode node, Port generatedPort)
        {
            var targetEdge = edges.ToList().Where(x =>
                x.output.portName == generatedPort.portName && x.output.node == generatedPort.node);
            if (targetEdge.Any())
            {
                Edge edge = targetEdge.First();
                edge.input.Disconnect(edge);
                RemoveElement(edge);
            }

            node.outputContainer.Remove(generatedPort);
            node.RefreshPorts();
            node.RefreshExpandedState();
        }
    }
}
