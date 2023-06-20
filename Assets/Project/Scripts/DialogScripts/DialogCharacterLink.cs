using System;
using Project.Scripts.Tiles;
using UnityEngine;

namespace Project.Scripts.DialogScripts
{
    public class DialogCharacterLink : CharacterLinkBase
    {
        private DialogManager myDialogManager;
        [SerializeField] private bool overrideCharacter;
        [SerializeField] private CharacterAnimator.Characters character;

        private void OnEnable()
        {
            myDialogManager = DialogManager.Instance;
            myDialogManager.OnNodeLoaded += LoadNewMoodFromDialogData;
        }

        private void OnDisable()
        {
            myDialogManager.OnNodeLoaded -= LoadNewMoodFromDialogData;
        }

        private void Start()
        {
            LoadDialogCharacter();
        }

        private void LoadDialogCharacter()
        {
            myCharacterAnimator.CurrentCharacter = overrideCharacter
                ? character
                : LevelDataLoader.Instance.GetLevelData(PlayerPrefs.GetInt("levelID", 0)).Character;

            myCharacterAnimator.CurrentMode = CharacterAnimator.CharacterMoods.Happy;
        }

        private void LoadNewMoodFromDialogData()
        {
            if(myDialogManager.CurrentNode.character != myCharacterAnimator.CurrentCharacter) return;
            ChangeCharacterMode(myDialogManager.CurrentNode.mood);
        }
    }
}
