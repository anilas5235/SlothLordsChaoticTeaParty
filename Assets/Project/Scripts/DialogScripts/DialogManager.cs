using System;
using System.Collections;
using System.Linq;
using Project.Scripts.General;
using UnityEngine;

namespace Project.Scripts.DialogScripts
{
    public class DialogManager : Singleton<DialogManager>
    {
        public event Action OnDialogStart;
        public event Action OnDialogEnd;

        public event Action OnNodeLoaded;
        public event Action<string> OnSpeakerChanged;
        public event Action<string> OnTextChanged;
        public event Action<string[]> OnChoice;
        public event Action OnChoiceOver;
        public event Action<AudioClip> OnVoiceLine;

        public float charactersPerSecond = 30;

        private Dialog currentStory;
        public Dialog CurrentStory { get => currentStory; } 
        private DialogPassageNode dialogPassageNode;
        public DialogPassageNode CurrentNode { get => dialogPassageNode; }

        private string speakerName;
        public bool FinishedLine { get; private set; }
        public bool DialogFinished { get; private set; }
        private bool  choiceMade = true;
        private string[] choices;
        public int CurrentDialogID { get; private set; }

        private Coroutine writeRoutine;

        private void Update()
        {
            if (Input.GetButtonDown("Jump") && choiceMade &&! DialogFinished) NextPassage();
        }

        public void StartDialog(string guid, int dialogID)
        {
            if (GetDialog(CurrentDialogID, out Dialog newDialog)) currentStory = newDialog;
            else return;
            CurrentDialogID = dialogID;
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
            speakerName =dialogPassageNode.character == CharacterAnimator.Characters.None ? dialogPassageNode.speaker : dialogPassageNode.character.ToString();
            OnSpeakerChanged?.Invoke(speakerName);

            // Case: Choices

            choices = new string[dialogPassageNode.links.Count];
            for (int i = 0; i < dialogPassageNode.links.Count; i++)
            {
                choices[i] = dialogPassageNode.links[i].OptionName;
            }
            writeRoutine = StartCoroutine(Write(dialogPassageNode.text, dialogPassageNode.links.Count > 1));

            if (dialogPassageNode.audioLine) OnVoiceLine?.Invoke(dialogPassageNode.audioLine);
            OnNodeLoaded?.Invoke();
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
            writeRoutine = null;
        }

        private void NextPassage(int linkID = 0)
        {
            if(writeRoutine!= null) StopCoroutine(writeRoutine);
            
            switch (dialogPassageNode.links.Count)
            {
                case 0: DialogFinished = true; OnDialogEnd?.Invoke(); break;
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

        public void SetDialogId(int id)
        {
            CurrentDialogID = id;
        }
    }
}