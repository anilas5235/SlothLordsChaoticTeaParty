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
            SaveData saveData = SaveSystem.Instance.GetActiveSave();
            score = (int) saveData.highScoresForLevels[levelID];
            levelData = LevelDataLoader.Instance.GetLevelData(levelID);
            float progress = (float) score / levelData.PerfectScore;
            progressBar.value = Mathf.Clamp(progress, 0f, 1f);
            
            if (score >= levelData.PerfectScore * .5f)
            {
                if (!saveData.unlockedSilverCrowns[levelID])
                    saveData.unlockedSilverCrowns[levelID] = true;
                if (!saveData.levelsUnlocked[levelID + 1])
                    saveData.levelsUnlocked[levelID + 1] = true;
            }

            if (score >= levelData.PerfectScore)
            {
                if (!saveData.unlockedGoldCrowns[levelID])
                    saveData.unlockedGoldCrowns[levelID] = true;
            }
            
            
            if (!saveData.unlockedEndings[0])
            {
                bool endingUnlock = true;
                for (int i = 0; i < 4; i++)
                {
                    if (saveData.unlockedSilverCrowns[i]) continue;
                    endingUnlock = false;
                    break;
                }
                if (endingUnlock) saveData.unlockedEndings[0] = true;
            }
                    
            if (!saveData.unlockedEndings[1])
            {
                bool endingUnlock = true;
                for (int i = 0; i < 4; i++)
                {
                    if (saveData.unlockedGoldCrowns[i]) continue;
                    endingUnlock = false;
                    break;
                }
                if (endingUnlock) saveData.unlockedEndings[1] = true;
            }
            
            //Set Up Character
            CharacterAnimator.CharacterMoods mood = CharacterAnimator.CharacterMoods.Neutral;
            if (saveData.unlockedSilverCrowns[levelID]) mood = CharacterAnimator.CharacterMoods.Party;
            else if (saveData.unlockedGoldCrowns[levelID]) mood = CharacterAnimator.CharacterMoods.Happy;
            characterAnimator.CurrentCharacter = levelData.Character;
            characterAnimator.CurrentMode = mood;

            //Set Up Stars
            star1.sprite = !saveData.unlockedSilverCrowns[levelID] ? stars[0] : stars[1];
            star2.sprite = !saveData.unlockedGoldCrowns[levelID] ? stars[0] : stars[2];
            
            scoreText.text = $"{score}";
            SaveSystem.Instance.Save();
        }
    }
}
