using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace Project.Scripts.DialogScripts
{
    [CreateAssetMenu] [Serializable]
    public class Dialog : ScriptableObject
    {
        public int id;
        public List<DialogPassageNode> passages;
        public int ID => id;

        public bool GetPassage(string guid,out DialogPassageNode wantedDialogPassageNode)
        {
            wantedDialogPassageNode = new DialogPassageNode();
            foreach (var dialogPassage in passages.Where(dialogPassage => dialogPassage.guid == guid))
            {
                wantedDialogPassageNode = dialogPassage;
                return true;
            }
            return false;
        }
    }

    [Serializable]
    public class DialogPassageNode
    {
        public string guid;
        public string speaker;
        public string text;
        public AudioClip audioLine;
        public List<Link> links;
        public Rect rect;
        public bool entryPoint;
        public CharacterAnimator.CharacterMoods mood;
        public Sprite imageOverride;
        public string GetLinkPassageGuid(int index) => links[index].Guid;
    }

    [Serializable]
    public struct Link
    {
        public string baseNodeGuid;
        public string portName;
        public string targetNodeGuid;
        
        public string OptionName=> portName; 
        public string Guid => targetNodeGuid;
    }
}