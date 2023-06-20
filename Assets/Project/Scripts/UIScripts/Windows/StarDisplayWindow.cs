using System.Xml;
using Project.Scripts.DialogScripts;
using Project.Scripts.General;
using Project.Scripts.Menu;
using Project.Scripts.Tiles;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.UIScripts.Windows
{
    public abstract class StarDisplayWindow : SingleWindow<StarDisplayWindow>
    {
        [SerializeField] protected Image star1, star2;
        [SerializeField] protected Slider progressBar;
        [SerializeField] protected TextMeshProUGUI scoreText;
        [SerializeField] protected CharacterAnimator characterAnimator;
        
        public int levelID,dialogID;
        [SerializeField] protected int score;
        [SerializeField] protected Level levelData;

        protected Sprite[] stars;

        protected override void Awake()
        {
            base.Awake();
            stars = Resources.LoadAll<Sprite>("ArtWork/UI/LevelProgressDisplay/crown_all");
        }

        public override void ActivateWindow()
        {
            base.ActivateWindow();
            UpdateDisplays();
        }

        protected abstract void UpdateDisplays();

        public void SetLeveVar(int lvlId)
        {
            levelID = lvlId;
        }
        
        public void Play()
        {
            if(levelID == 0)
            {
                SceneMaster.Instance.LoadLevel();
                SaveData data = SaveSystem.Instance.GetActiveSave();
                if (!data.firstTimeTutorialDone)
                {
                    data.firstTimeTutorialDone = true;
                    SaveSystem.Instance.Save();
                }
            }
            else
            {
                SceneMaster.Instance.ChangeToLevelDialog(levelID, LevelDataLoader.Instance.GetLevelData(levelID).intro);
            }
        }

        public virtual void PlayEndDialog()
        {
            
        }
    }
}
