using System.Collections;
using Project.Scripts.Audio;
using Project.Scripts.DialogScripts;
using Project.Scripts.Menu;
using Project.Scripts.Tiles;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.UIScripts.Windows
{
    public class LevelEndWindow : StarDisplayWindow
    {
        private TileFieldManager fieldManager;
        private bool star1Achieved , star2Achieved , doneRevealing;
        
        protected override void UpdateDisplays()
        {
            fieldManager ??= TileFieldManager.Instance;
            levelID = fieldManager.currentLevelID;
            levelData = fieldManager.CurrentLevelData;
            score = fieldManager.Score;
            star1.sprite = star2.sprite = stars[0];
            characterAnimator.CurrentCharacter = levelData.Character;
            characterAnimator.CurrentMode = CharacterAnimator.CharacterMoods.Neutral;
            StartCoroutine(SlowReveal());
        }

        private IEnumerator SlowReveal()
        {
            Interactable = false;
            float t = 0;
            while (t<=1)
            {
                float progress = (float) score / levelData.PerfectScore;
                scoreText.text = $"{(int)(score * t)}";
                progressBar.value = Mathf.Clamp(progress * t, 0f, 1f);
                if (!(progress < .5f) && !star1Achieved)
                {
                     star1.sprite = stars[1];
                     characterAnimator.CurrentMode = CharacterAnimator.CharacterMoods.Happy;
                     star1Achieved = true;
                }
                else if(!(progress < 1f) && !star2Achieved)
                {
                    star2.sprite = stars[2];
                    characterAnimator.CurrentMode = CharacterAnimator.CharacterMoods.Party;
                    star2Achieved = true;
                }

                t += .01f;
                yield return new WaitForFixedUpdate();
            }

            scoreText.text = $"{(score)}";
            int s = 0;
            if (star1Achieved) s++;
            if (star2Achieved) s++;
            AudioManager.Instance.TriggerEndSound(s);
            Interactable = true;
            doneRevealing = true;
        }

        public override void UIEsc() { }

        public override void SwitchToMainMenu()
        {
            if(!doneRevealing) return;
            base.SwitchToMainMenu();
        }

        public override void PlayEndDialog()
        {
            if(!doneRevealing) return;
            if(star2Achieved){SceneMaster.Instance.ChangeToLevelDialog(fieldManager.currentLevelID,fieldManager.CurrentLevelData.perfectEnding);}
            else if(star1Achieved) { SceneMaster.Instance.ChangeToLevelDialog(fieldManager.currentLevelID,fieldManager.CurrentLevelData.goodEnding); }
            else { SceneMaster.Instance.ChangeToLevelDialog(fieldManager.currentLevelID,fieldManager.CurrentLevelData.badEnding); }
        }
    }
}
