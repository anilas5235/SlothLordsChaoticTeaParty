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

            switch (myDialogManager.CurrentNode.character)
            {
                case CharacterAnimator.Characters.None: return;
                case CharacterAnimator.Characters.Elenor:
                    break;
                case CharacterAnimator.Characters.Gonzo:
                    break;
                case CharacterAnimator.Characters.Norbert:
                    break;
                case CharacterAnimator.Characters.Sheldon:
                    break;
                case CharacterAnimator.Characters.Lazy: return;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            myCharacterAnimator.CurrentCharacter = myDialogManager.CurrentNode.character;
            ChangeCharacterMode(myDialogManager.CurrentNode.mood);
        }
    }
}
