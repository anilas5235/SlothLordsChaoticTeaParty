using System;
using Project.Scripts.Tiles;
using TMPro;
using UnityEngine;

namespace Project.Scripts.General
{
    public class UIManager : Singleton<UIManager>
    {
        [SerializeField] private TextMeshProUGUI scoreText;

        private void Start()
        {
            UpdateScore();
        }

        public void UpdateScore()
        {
            scoreText.text = $"Score: {TileManager.instance.Score}";
        }

    }
}
