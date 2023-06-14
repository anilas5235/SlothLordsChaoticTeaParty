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
        }

        private void OnDisable()
        {
            tileFieldManager.OnCombo -= ChangeCharacterMode;
        }

        public void ChangeCharacterMode(TileFieldManager.ComboAppraisal appraisal)
        {
            CharacterAnimator.CharacterMods mode = appraisal switch
            {
                TileFieldManager.ComboAppraisal.Neutral => CharacterAnimator.CharacterMods.Neutral,
                TileFieldManager.ComboAppraisal.Good => CharacterAnimator.CharacterMods.Happy,
                TileFieldManager.ComboAppraisal.Bad => CharacterAnimator.CharacterMods.Offended,
                TileFieldManager.ComboAppraisal.Party => CharacterAnimator.CharacterMods.Party,
                _ => throw new ArgumentOutOfRangeException(nameof(appraisal), appraisal, null)
            };

            myCharacterAnimator.CurrentMode = mode;
        }
    }
}
