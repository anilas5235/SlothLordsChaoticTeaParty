using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Project.Scripts.DialogScripts.Editor
{
    public class DialogNode : Node
    {
        public string guid;

        public string dialogText;

        public string speaker;

        public AudioClip voiceLine;

        public bool entryPoint;
        
        public CharacterAnimator.CharacterMoods mood;
        
        public Sprite imageOverride;
    }
}
