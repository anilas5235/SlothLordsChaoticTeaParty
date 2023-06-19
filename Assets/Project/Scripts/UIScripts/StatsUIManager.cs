using System;
using Project.Scripts.General;
using Project.Scripts.Tiles;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.UIScripts
{
    public class StatsUIManager : Singleton<StatsUIManager>
    {
        [SerializeField] private TextMeshProUGUI scoreText, turnText, passingScore,perfectScore;
        [SerializeField] private Slider progressBar;
        [SerializeField] private Image likeTile, dislikeTile, star1, star2;
        private TileFieldManager FieldManager => TileFieldManager.Instance;

        private Sprite[] stars;

        protected override void Awake()
        {
            base.Awake();
            stars = Resources.LoadAll<Sprite>("ArtWork/UI/LevelProgressDisplay/crown_all");
        }

        private void OnEnable()
        {
            FieldManager.OnGameStart += UpdateAllUIFields;
        }
        public void UpdateScore(int score)
        {
            scoreText.text = $"{score}";
            UpdateProgress(score);
        }

        public void UpdateTurn(int turns) => turnText.text = $"{turns}";

        private void UpdateProgress(int score)
        {
            Level data = FieldManager.CurrentLevelData;
            float progress = (float) score / data.PerfectScore;
            progressBar.value = Mathf.Clamp(progress, 0f, 1f);
            star1.sprite = progress < 0.5f? stars[0] : stars[1];
            star2.sprite = progress < 1? stars[0] : stars[2];
        }

        private void SetPrefImages()
        {
            likeTile.sprite = TileRecourseKeeper.Instance.tileSprites[(int)FieldManager.preferredTile];
            dislikeTile.sprite = TileRecourseKeeper.Instance.tileSprites[(int)FieldManager.dislikedTile];
        }

        private void SetScoreTipTextFields()
        {
            passingScore.text = $"{(int)FieldManager.CurrentLevelData.PerfectScore * .5f}";
            perfectScore.text = $"{FieldManager.CurrentLevelData.PerfectScore }";
        }

        private void UpdateAllUIFields()
        {
            UpdateScore(FieldManager.Score);
            UpdateTurn(FieldManager.Turns);
            SetPrefImages();
            SetScoreTipTextFields();
        }

    }
}
