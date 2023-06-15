using System;
using Project.Scripts.Tiles;
using UnityEngine;

namespace Project.Scripts.DialogScripts
{
    public class LevelCharacterLink : MonoBehaviour
    {
        [SerializeField] private CharacterAnimator myCharacterAnimator;
        private TileFieldManager tileFieldManager;

        private void Awake()
        {
            myCharacterAnimator = GetComponent<CharacterAnimator>();
        }

        private void Start()
        {
            tileFieldManager = TileFieldManager.Instance;
            tileFieldManager.OnCombo += ChangeCharacterMode;
            tileFieldManager.OnSelectCharacter += SelectCharacter;
        }

        private void OnDisable()
        {
            tileFieldManager.OnCombo -= ChangeCharacterMode;
            tileFieldManager.OnSelectCharacter -= SelectCharacter;
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

        private void SelectCharacter(CharacterAnimator.Characters character)
        {
            myCharacterAnimator.CurrentCharacter = character;
        }
    }
}
