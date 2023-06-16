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
            star1.sprite = progress < 0.5f? stars[0] : stars[1];
            star2.sprite = progress < 1? stars[0] : stars[2];
        }
    }
}
