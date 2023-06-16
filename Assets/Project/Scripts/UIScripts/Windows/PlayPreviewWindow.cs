using Project.Scripts.DialogScripts;
using Project.Scripts.General;
using Project.Scripts.Tiles;
using UnityEngine;

namespace Project.Scripts.UIScripts.Windows
{
    public class PlayPreviewWindow : StarDisplayWindow
    {
        protected override void UpdateDisplays()
        {
            score = (int) SaveSystem.Instance.GetActiveSave().highScoresForLevels[levelID];
            levelData = LevelDataLoader.Instance.GetLevelData(levelID);
            float progress = (float) score / levelData.PerfectScore;
            progressBar.value = Mathf.Clamp(progress, 0f, 1f);
            
            //Set Up Character
            CharacterAnimator.CharacterMoods mood = CharacterAnimator.CharacterMoods.Neutral;
            if (progress >= 0.5f) mood = CharacterAnimator.CharacterMoods.Happy;
            else if (progress >= 1f) mood = CharacterAnimator.CharacterMoods.Party;
            characterAnimator.CurrentCharacter = levelData.Character;
            characterAnimator.CurrentMode = mood;

            //Set Up Stars
            star1.sprite = progress < 0.5f? stars[0] : stars[1];
            star2.sprite = progress < 1? stars[0] : stars[2];
            
            scoreText.text = $"{score}";
        }
    }
}
