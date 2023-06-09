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
        private Passage passage;

        private string speakerName;
        public bool FinishedLine { get; private set; }
        private bool  choiceMade = true;
        private string[] choices;
        private int currentDialogID;

        private void Update()
        {
            if (Input.GetButtonDown("Jump") && FinishedLine && choiceMade) NextPassage();
        }

        public void StartDialog(int pid, int dialogID)
        {
            if (GetDialog(currentDialogID, out Dialog newDialog)) currentStory = newDialog;
            else return;
            currentDialogID = dialogID;
            OnDialogStart?.Invoke();
            LoadPassage(pid);
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

        private bool LoadPassage(int pid)
        {
            // Get Data

            if (currentStory.GetPassage(pid, out Passage nextPassage))
                passage = nextPassage;
            else return false;

            // Handle Speaker Name
            speakerName = passage.Speaker;
            OnSpeakerChanged?.Invoke(speakerName);

            // Case: Choices

            choices = new string[passage.Links.Count];
            for (int i = 0; i < passage.Links.Count; i++)
            {
                choices[i] = passage.Links[i].OptionName;
            }

            StartCoroutine(Write(passage.Text, passage.Links.Count > 1));

            if (passage.AudioLine) OnVoiceLine?.Invoke(passage.AudioLine);
            
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
            switch (passage.Links.Count)
            {
                case 0: OnDialogEnd?.Invoke(); break;
                case 1: LoadPassage(passage.Links[0].Pid);break;
                default: LoadPassage(passage.Links[linkID].Pid); break;
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