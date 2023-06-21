using System.Collections;
using Project.Scripts.General;
using Project.Scripts.Menu;
using Project.Scripts.Tiles;
using Project.Scripts.UIScripts.Windows;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.DialogScripts
{
    public class DialogHandler : Singleton<DialogHandler>
    {
        private DialogManager dialogManager;
        
        [Header("TutorialMode")]
        [SerializeField] private bool tutorialMode;
        [SerializeField] private bool inMenu;

        [SerializeField] private GameObject dialogUIParent;
        
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

        [Header("Image")] 
        [SerializeField] private Image screenSceneImage;

        private bool isAllowedToWrite = true;
        
        private void Start()
        {
            dialogManager = DialogManager.Instance;
            dialogManager.OnDialogStart += ClearTextFields;
            dialogManager.OnSpeakerChanged += SetNameText;
            dialogManager.OnTextChanged += SetMainText;
            dialogManager.OnChoice += ChoiceHandler;
            dialogManager.OnChoiceOver += ChoiceMade;
            dialogManager.OnDialogEnd += ClearTextFields;
            dialogManager.OnVoiceLine += PlayVoiceLine;
            dialogManager.OnDialogEnd += DialogEnded;
            ChoiceMade();
            if (inMenu)
            {
                if (SaveSystem.Instance.GetActiveSave().firstTimeTutorialDone)
                {
                    dialogUIParent.gameObject.SetActive(false);
                                    return;
                }
                else
                {
                    if (MenuWindowsMaster.Instance) MenuWindowsMaster.Instance.enabled = false;
                }
            }
            LoadDialogID();
            StartCoroutine( StartDialogAfterFade());
        }

        private void Update()
        {
            if (!dialogManager.FinishedLine) scrollbar.value = 0;
        }

        public void StartTheDialog()
        {
            dialogManager.StartDialog(startPassageGuid,dialogId);
        }

        private void LoadDialogID()
        {
            dialogId = tutorialMode? dialogId: PlayerPrefs.GetInt("DialogID");
            dialogManager.SetDialogId(dialogId);
        }
        private void SetNameText(string name)
        {
            SetFullSceneImage();
            if (!isAllowedToWrite)return;
            nameText.text = name;
        }

        private void SetMainText(string text)
        {
            if (!isAllowedToWrite)return;
            mainText.text = text;
        }

        private void ClearTextFields()
        {
            nameText.text = "";
            mainText.text = "";
        }

        private void PlayVoiceLine(AudioClip voiceLine)
        {
            audioSource.clip = (voiceLine);
            audioSource.Play();
        }

        private void ChoiceHandler(string[] choicesTexts)
        {
            for (int i = 0; i < choicesTexts.Length; i++)
            {
                if(choiceButtons[i] == null) continue;
                
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

        private void SetFullSceneImage()
        {
            if(!screenSceneImage) return;
            if (dialogManager.CurrentNode.imageOverride)
            {
                isAllowedToWrite = false;
                if (MenuWindowsMaster.Instance) MenuWindowsMaster.Instance.enabled = true;
                screenSceneImage.gameObject.SetActive(true);
                screenSceneImage.sprite = dialogManager.CurrentNode.imageOverride;
                ClearTextFields();
            }
            else
            {
                isAllowedToWrite = true;
                screenSceneImage.gameObject.SetActive(false);
            }
        }

        private IEnumerator StartDialogAfterFade()
        {
            if(ScreenFade.Instance) yield return new WaitForSeconds(ScreenFade.Instance.FadeDuration);
            StartTheDialog();
        }

        private void DialogEnded()
        {
            if (tutorialMode)
            {
                if(TileFieldManager.Instance)TileFieldManager.Instance.tutorialMode = false;
                if (inMenu)
                {
                    FirstPersonController player = FindObjectOfType<FirstPersonController>();
                    if(player)player.SetJumpAbility(true); 
                }
               
                dialogUIParent.gameObject.SetActive(false);
                return;
            }
            Level currenLevelData = LevelDataLoader.Instance.GetLevelData(PlayerPrefs.GetInt("levelID", 0));
            StartCoroutine(currenLevelData.intro == dialogManager.CurrentDialogID ? FadeAndContinueToLevel() : FadeAndContinueToMenu());
        }

        private IEnumerator FadeAndContinueToLevel()
        {
            ScreenFade.Instance.StartFadeOut();
            yield return new WaitForSeconds(ScreenFade.Instance.FadeDuration);
            SceneMaster.Instance.LoadLevel();
        }
        
        private IEnumerator FadeAndContinueToMenu()
        {
            ScreenFade.Instance.StartFadeOut();
            yield return new WaitForSeconds(ScreenFade.Instance.FadeDuration);
            SceneMaster.Instance.ChangeToMenuScene();
        }
    }
}
