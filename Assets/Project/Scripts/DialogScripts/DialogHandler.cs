using System.Collections.Generic;
using Project.Scripts.General;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Project.Scripts.DialogScripts
{
    public class DialogHandler : Singleton<DialogHandler>
    {
        private DialogManager dialogManager;
        [Header("ID´s")]
        [SerializeField] private int dialogId;
        [SerializeField] private string startPassageGuid;

        [Header("TextField´s")]
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI mainText;
        [SerializeField] private Scrollbar scrollbar;

        [Header("ChoiceButtons")] [SerializeField]
        private ChoiceButton[] choiceButtons;

        [Header("Audio")]
        [SerializeField] private AudioSource audioSource;

        private void Start()
        {
            dialogManager = DialogManager.Instance;
            dialogManager.OnDialogStart += DialogStarted;
            dialogManager.OnSpeakerChanged += SetNameText;
            dialogManager.OnTextChanged += SetMainText;
            dialogManager.OnChoice += ChoiceHandler;
            dialogManager.OnChoiceOver += ChoiceMade;
            dialogManager.OnDialogEnd += DialogEnded;
            dialogManager.OnVoiceLine += PlayVoiceLine;
            dialogManager.StartDialog(startPassageGuid,dialogId);
            ChoiceMade();
        }

        private void Update()
        {
            if (!dialogManager.FinishedLine) scrollbar.value = 0;
        }

        private void SetNameText(string name)
        {
            nameText.text = name;
        }

        private void SetMainText(string text)
        {
            mainText.text = text;
        }

        private void DialogStarted()
        {
            nameText.text = "";
            mainText.text = "";
        }

        private void DialogEnded()
        {
            nameText.text = "";
            mainText.text = "";
        }

        private void PlayVoiceLine(AudioClip voiceLine)
        {
            audioSource.PlayOneShot(voiceLine);
        }

        private void ChoiceHandler(string[] choicesTexts)
        {
            for (int i = 0; i < choicesTexts.Length; i++)
            {
                choiceButtons[i].gameObject.SetActive(true);
                choiceButtons[i].buttonText.text = choicesTexts[i];
            } 
        }

        private void ChoiceMade()
        {
            foreach (var choiceB in choiceButtons)
            {
                if (choiceB.gameObject.activeSelf) choiceB.gameObject.SetActive(false);
                else break;
            }
        }
    }
}
