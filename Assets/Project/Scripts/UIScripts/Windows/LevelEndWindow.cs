using System.Collections;
using Project.Scripts.DialogScripts;
using Project.Scripts.Tiles;
using UnityEngine;

namespace Project.Scripts.UIScripts.Windows
{
    public class LevelEndWindow : StarDisplayWindow
    {
        private TileFieldManager fieldManager;
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
            float t = 0;
            bool star1Achieved = false, star2Achieved = false;
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
        }
    }
}
