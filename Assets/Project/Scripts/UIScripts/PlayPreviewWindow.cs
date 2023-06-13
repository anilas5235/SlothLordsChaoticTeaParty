using Project.Scripts.General;
using Project.Scripts.Menu;
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

        public void UpdateDisplays()
        {
            int currentHighScore = (int) SaveSystem.instance.GetActiveSave().highScoresForLevels[levelID];
        }

        public void Play()
        {
            SceneMaster.Instance.ChangeToLevel(levelID);   
        }
    }
}
