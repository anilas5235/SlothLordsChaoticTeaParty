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
            SaveData save = SaveSystem.Instance.GetActiveSave();
            score = (int) save.highScoresForLevels[levelID];
            levelData = LevelDataLoader.Instance.GetLevelData(levelID);
            float progress = (float) score / levelData.PerfectScore;
            progressBar.value = Mathf.Clamp(progress, 0f, 1f);
            
            //Set Up Character
            CharacterAnimator.CharacterMoods mood = CharacterAnimator.CharacterMoods.Neutral;
            if (save.unlockedSilverCrowns[levelID]) mood = CharacterAnimator.CharacterMoods.Party;
            else if (save.unlockedGoldCrowns[levelID]) mood = CharacterAnimator.CharacterMoods.Happy;
            characterAnimator.CurrentCharacter = levelData.Character;
            characterAnimator.CurrentMode = mood;

            //Set Up Stars
            star1.sprite = save.unlockedSilverCrowns[levelID] ? stars[0] : stars[1];
            star2.sprite = save.unlockedGoldCrowns[levelID] ? stars[0] : stars[2];
            
            scoreText.text = $"{score}";
        }
    }
}
