using System;

namespace Project.Scripts.DialogScripts
{
    public class DialogCharacterLink : CharacterLinkBase
    {
        private DialogManager myDialogManager;

        private void OnEnable()
        {
            myDialogManager = DialogManager.Instance;
            myDialogManager.OnDialogStart += LoadDialogCharacter;
            myDialogManager.OnNodeLoaded += LoadNewMoodFromDialogData;
        }

        private void OnDisable()
        {
            myDialogManager.OnDialogStart -= LoadDialogCharacter;
            myDialogManager.OnNodeLoaded -= LoadNewMoodFromDialogData;
        }

        private void LoadDialogCharacter()
        {
            SelectCharacter(myDialogManager.CurrentStory.dialogCharacter);
            ChangeCharacterMode(CharacterAnimator.CharacterMoods.Neutral);
        }

        private void LoadNewMoodFromDialogData()
        {
            if(myDialogManager.CurrentNode.character != myCharacterAnimator.CurrentCharacter) return;
            ChangeCharacterMode(myDialogManager.CurrentNode.mood);
        }
    }
}
