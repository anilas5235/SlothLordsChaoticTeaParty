using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Project.Scripts.DialogScripts
{
    [CreateAssetMenu]
    public class Dialog : ScriptableObject
    {
        [SerializeField] private int id;
        [SerializeField] private List<Passage> passages;
        
        public int ID { get => id; }

        public bool GetPassage(int pid,out Passage wantedPassage)
        {
            wantedPassage = new Passage();
            foreach (var dialogPassage in passages.Where(dialogPassage => dialogPassage.Pid == pid))
            {
                wantedPassage = dialogPassage;
                return true;
            }
            return false;
        }
    }

    [Serializable]
    public struct Passage
    {
        [SerializeField] private int pid;
        [SerializeField] private string speaker;
        [SerializeField] private string text;
        [SerializeField] private AudioClip audioLine;
        [SerializeField] private List<Link> links;
        
        public int Pid { get => pid; }
        public string Text { get => text; }
        public string Speaker { get => speaker; }
        public List<Link> Links { get => links; }
        public AudioClip AudioLine { get => audioLine; }

        public int GetLinkPassageID(int index) => links[index].Pid;
    }

    [Serializable]
    public struct Link
    {
        [SerializeField] private string optionName;
        [SerializeField] private int pid;
        
        public string OptionName { get => optionName; }
        public int Pid { get => pid; }
    }
}