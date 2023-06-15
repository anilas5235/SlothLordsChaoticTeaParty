using Project.Scripts.General;
using Project.Scripts.Menu;
using Project.Scripts.Tiles;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.UIScripts.Windows
{
    public abstract class StarDisplayWindow : SingleWindow<StarDisplayWindow>
    {
        [SerializeField] protected Image star1, star2;
        [SerializeField] protected Slider progressBar;
        
        public int levelID;
        [SerializeField] protected int score;
        [SerializeField] protected Level levelData;

        protected Sprite[] stars;

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

        protected abstract void UpdateDisplays();

        public void Play()
        {
            SceneMaster.Instance.ChangeToLevel(levelID);   
        }
    
    }
}
