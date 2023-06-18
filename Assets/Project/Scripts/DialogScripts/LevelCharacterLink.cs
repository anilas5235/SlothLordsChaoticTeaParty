using System;
using Project.Scripts.Tiles;
using UnityEngine;

namespace Project.Scripts.DialogScripts
{
    public class LevelCharacterLink : CharacterLinkBase
    {
        private TileFieldManager tileFieldManager;
        private void OnEnable()
        {
            tileFieldManager = TileFieldManager.Instance;
            tileFieldManager.OnCombo += ChangeCharacterMode;
            tileFieldManager.OnSelectCharacter += SelectCharacter;
            tileFieldManager.OnDoneFalling += Calm;
        }

        private void OnDisable()
        {
            tileFieldManager.OnCombo -= ChangeCharacterMode;
            tileFieldManager.OnSelectCharacter -= SelectCharacter;
            tileFieldManager.OnDoneFalling -= Calm;
        }

        private void ChangeCharacterMode(TileFieldManager.ComboAppraisal appraisal)
        {
            CharacterAnimator.CharacterMoods mode = appraisal switch
            {
                TileFieldManager.ComboAppraisal.Neutral => CharacterAnimator.CharacterMoods.Neutral,
                TileFieldManager.ComboAppraisal.Good => CharacterAnimator.CharacterMoods.Happy,
                TileFieldManager.ComboAppraisal.Bad => CharacterAnimator.CharacterMoods.Offended,
                TileFieldManager.ComboAppraisal.Party => CharacterAnimator.CharacterMoods.Party,
                _ => throw new ArgumentOutOfRangeException(nameof(appraisal), appraisal, null)
            };

            myCharacterAnimator.CurrentMode = mode;
        }
        private void Calm()
        {
            if(myCharacterAnimator.CurrentMode == CharacterAnimator.CharacterMoods.Party)
                myCharacterAnimator.CurrentMode = CharacterAnimator.CharacterMoods.Happy;
        }
    }
}
