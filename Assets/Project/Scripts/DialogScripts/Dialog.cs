using System;
using System.Collections.Generic;

namespace Project.Scripts.DialogScripts
{
    public class Dialog
    {
        public List<Passage> passages;

        public Passage GetPassage(int pid)
        {
            Passage passage = new Passage();
            foreach (Passage dialogPassage in passages)
            {
                if (dialogPassage.pid == pid)
                {
                    passage = dialogPassage;
                    break;
                }
            }
            return passage;
        }
    }

    [Serializable]
    public struct Passage
    {
        public string text;
        public List<Link> links;
        public int pid;
        public List<string> tags;

        public int GetLinkPassageID(int index) => links[index].pid;
    }

    [Serializable]
    public struct Link
    {
        public string name;
        public int pid;
    }
}