using System;
using System.Collections;
using System.Linq;
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
        public event Action<string[]> OnChoice;
        public event Action OnChoiceOver;
        public event Action<AudioClip> OnVoiceLine;

        public float charactersPerSecond = 30;

        private Dialog currentStory;
        private DialogPassageNode dialogPassageNode;

        private string speakerName;
        public bool FinishedLine { get; private set; }
        private bool  choiceMade = true;
        private string[] choices;
        private int currentDialogID;

        private void Update()
        {
            if (Input.GetButtonDown("Jump") && FinishedLine && choiceMade) NextPassage();
        }

        public void StartDialog(string guid, int dialogID)
        {
            if (GetDialog(currentDialogID, out Dialog newDialog)) currentStory = newDialog;
            else return;
            currentDialogID = dialogID;
            OnDialogStart?.Invoke();
            if (string.IsNullOrEmpty(guid))
            {
                guid = currentStory.passages.First(x => x.entryPoint).guid;
            }
            LoadPassage(guid);
        }

        private bool GetDialog(int id, out Dialog wantedDialog)
        {
            wantedDialog = null;
            Dialog[] dialogs = Resources.LoadAll<Dialog>("Dialogs");

            foreach (Dialog dialog in dialogs.Where( dialog => dialog.ID == id))
            {
                wantedDialog = dialog;
                return true;
            }

            return false;
        }

        private bool LoadPassage(string guid)
        {
            // Get Data

            if (currentStory.GetPassage(guid, out DialogPassageNode nextPassage))
                dialogPassageNode = nextPassage;
            else return false;
            
            if(dialogPassageNode.entryPoint) 
            {
                NextPassage();
                return true;
            }

            // Handle Speaker Name
            speakerName = dialogPassageNode.speaker;
            OnSpeakerChanged?.Invoke(speakerName);

            // Case: Choices

            choices = new string[dialogPassageNode.links.Count];
            for (int i = 0; i < dialogPassageNode.links.Count; i++)
            {
                choices[i] = dialogPassageNode.links[i].OptionName;
            }

            StartCoroutine(Write(dialogPassageNode.text, dialogPassageNode.links.Count > 1));

            if (dialogPassageNode.audioLine) OnVoiceLine?.Invoke(dialogPassageNode.audioLine);
            
            return true;
        }

        private IEnumerator Write(string completeText, bool choice)
        {
            FinishedLine = false;
            string currentText = "";
            foreach (char letter in completeText)
            {
                currentText += letter;
                OnTextChanged?.Invoke(currentText);
                yield return new WaitForSeconds(1 / charactersPerSecond);
            }

            if (choice)
            {
                choiceMade = false;
                OnChoice?.Invoke(choices);
            }

            FinishedLine = true;
        }

        private void NextPassage(int linkID = 0)
        {
            switch (dialogPassageNode.links.Count)
            {
                case 0: OnDialogEnd?.Invoke(); break;
                case 1: LoadPassage(dialogPassageNode.links[0].Guid);break;
                default: LoadPassage(dialogPassageNode.links[linkID].Guid); break;
            }
        }

        public void Choose(int choiceId)
        {
            choiceMade = true;
            OnChoiceOver?.Invoke();
            NextPassage(choiceId);
        }
    }
}