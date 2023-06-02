using System;
using System.Collections;
using Project.Scripts.General;
using UnityEngine;

namespace Project.Scripts.DialogScripts
{
    public class DialogManager : Singleton<DialogManager>
    {
        public event Action<string> OnSpeakerChanged;
        public event Action<string> OnTextChanged;
        public event Action OnDialogStart;
        public event Action OnDialogEnd;
        public event Action<string, string> OnChoice;
        public event Action OnChoiceOver; 

        public float CharactersPerSecond = 40;

        private TextAsset[] stories;
        private Dialog dialog;
        private Passage passage;

        private string speakerName;
        private bool finishedLine;
        private string choice1;
        private string choice2;

        private int currentDialogID;

        protected override void Awake()
        {
            base.Awake();
            stories = Resources.LoadAll<TextAsset>("Dialogs");
        }

        private void Update()
        {
            if (Input.GetButtonDown("Jump") && finishedLine)
            {
                NextPassage();
            }
        }

        public void StartDialog(int pid, int dialogID)
        {
            OnDialogStart?.Invoke();
            currentDialogID = dialogID;
            CompilePassage(pid);
        }

        private void CompilePassage(int pid)
        {
            finishedLine = false;
            
            // Get Data
            dialog = JsonUtility.FromJson<Dialog>(stories[currentDialogID].text);
            passage = dialog.GetPassage(pid);
            string text = passage.text;

            // Handle Speaker Name
            int charsToDelete = 0;
            speakerName = "";
            for (int i = 0; i < text.Length; i++)
            {
                charsToDelete++;
                if (text[i] == ':') break;
                speakerName += text[i];
            }
            text = text.Remove(0, charsToDelete + 1);
            OnSpeakerChanged?.Invoke(speakerName);
            
            /*
             * Handle Main Text
             */
            string result = "";
            
            // Case: Choices
            if (passage.links.Count == 2)
            {
                choice1 = "";
                choice2 = "";
                
                int index = 0;
                for (int i = 0; i < text.Length; i++)
                {
                    if (text[i] == '[')
                    {
                        index = i;
                        break;
                    }
                    result += text[i];
                }

                int openBracketsCount = 0;
                int closedBracketsCount = 0;
                for (int i = index; i < text.Length; i++)
                {
                    if(text[i] == '[')
                    {
                        openBracketsCount++;
                        continue;
                    }

                    if (text[i] == ']')
                    {
                        closedBracketsCount++;
                        continue;
                    }

                    if (openBracketsCount == 2 && closedBracketsCount == 0) choice1 += text[i];
                    if (openBracketsCount == 4 && closedBracketsCount == 2) choice2 += text[i];
                }
            }
            
            // Case: No Choices
            if (passage.links.Count <= 1)
            {
                for (int i = 0; i < text.Length; i++)
                {
                    if (text[i] == '[') break;
                    result += text[i];
                }
            }

            StartCoroutine(Write(result, passage.links.Count > 1));
        }

        private IEnumerator Write(string result, bool choices)
        {
            string text = "";
            for (int i = 0; i < result.Length; i++)
            {
                text += result[i];
                OnTextChanged?.Invoke(text);
                yield return new WaitForSeconds(1 / CharactersPerSecond);
            }
            
            if(choices) OnChoice?.Invoke(choice1, choice2);

            finishedLine = true;
        }

        private void NextPassage(int linkID = 0)
        {
            if (passage.links.Count == 0)
            {
                OnDialogEnd.Invoke();
            }
            else if (passage.links.Count == 1)
            {
                CompilePassage(passage.links[0].pid);
            }
            else if (passage.links.Count == 2)
            {
                CompilePassage(passage.links[linkID].pid);
            }
        }

        public void Choose(int choose)
        {   
            OnChoiceOver?.Invoke();
            NextPassage(choose);
        }
    }
}