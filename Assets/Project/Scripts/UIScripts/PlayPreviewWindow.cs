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
            stars = new[]
            {
                Resources.Load<Sprite>("Artwork/UI/LevelProgressDisplay/emptystar"),
                Resources.Load<Sprite>("Artwork/UI/LevelProgressDisplay/star1"),
                Resources.Load<Sprite>("Artwork/UI/LevelProgressDisplay/star2"),
            };
        }

        public override void ActivateWindow()
        {
            base.ActivateWindow();
            UpdateDisplays();
        }

        private void UpdateDisplays()
        {
            int currentHighScore = (int) SaveSystem.instance.GetActiveSave().highScoresForLevels[levelID];
            Level data = LevelDataLoader.Instance.GetLevelData(levelID);
            progressBar.value = Mathf.Clamp(currentHighScore / (data.LevelSuccessScore * 1.1f), 0f, 1f);
            star1.sprite = currentHighScore < data.LevelCompleteScore ? stars[0] : stars[1];
            star2.sprite = currentHighScore < data.LevelSuccessScore ? stars[0] : stars[2];
        }

        public void Play()
        {
            SceneMaster.Instance.ChangeToLevel(levelID);   
        }
    }
}
