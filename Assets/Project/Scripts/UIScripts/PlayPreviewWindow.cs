using Project.Scripts.General;
using Project.Scripts.Menu;
using Project.Scripts.Tiles;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.UIScripts
{
    public class PlayPreviewWindow : SingleWindow<PlayPreviewWindow>
    {
        [SerializeField] private Image star1, star2;
        [SerializeField] private Slider progressBar;
        public int levelID;

        private Sprite[] stars;

        protected override void Awake()
        {
            base.Awake();
            stars = Resources.LoadAll<Sprite>("ArtWork/UI/LevelProgressDisplay/Stars");
        }

        public override void ActivateWindow()
        {
            base.ActivateWindow();
            UpdateDisplays();
        }

        private void UpdateDisplays()
        {
            int currentHighScore = (int) SaveSystem.Instance.GetActiveSave().highScoresForLevels[levelID];
            Level data = LevelDataLoader.Instance.GetLevelData(levelID);
            float progress = (float) currentHighScore / data.PerfectScore;
            progressBar.value = Mathf.Clamp(progress, 0f, 1f);
            star1.sprite = progress < 0.5f? stars[0] : stars[1];
            star2.sprite = progress < 0.99f? stars[0] : stars[2];
        }

        public void Play()
        {
            SceneMaster.Instance.ChangeToLevel(levelID);   
        }
    }
}
