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
        [SerializeField] private TextMeshProUGUI scoreText, turnText ;
        [SerializeField] private Slider progressBar;
        [SerializeField] private Image likeTile, dislikeTile;
        private TileFieldManager FieldManager => TileFieldManager.Instance;

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
        }

        private void SetPrefImages()
        {
            likeTile.sprite = TileRecourseKeeper.Instance.tileSprites[(int)FieldManager.preferredTile];
            dislikeTile.sprite = TileRecourseKeeper.Instance.tileSprites[(int)FieldManager.dislikedTile];
        }

        private void UpdateAllUIFields()
        {
            UpdateScore(FieldManager.Score);
            UpdateTurn(FieldManager.Turns);
            SetPrefImages();
        }

    }
}
