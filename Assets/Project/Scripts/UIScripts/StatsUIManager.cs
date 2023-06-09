using Project.Scripts.General;
using Project.Scripts.Tiles;
using TMPro;
using UnityEngine;

namespace Project.Scripts.UIScripts
{
    public class StatsUIManager : Singleton<StatsUIManager>
    {
        [SerializeField] private TextMeshProUGUI scoreText, comboRollText, turnText ;

        private void Start()
        {
            UpdateAllUIFields();
        }

        public void UpdateScore()
        {
            scoreText.text = $"Score: {TileFieldManager.Instance.Score}";
        }

        public void UpdateComboRoll()
        {
            comboRollText.text = $"Combos: {TileFieldManager.Instance.ComboRoll}";
        }

        public void UpdateTurn()
        {
            turnText.text = $"Turns: {TileFieldManager.Instance.turns}";
        }

        public void UpdateAllUIFields()
        {
            UpdateScore();
            UpdateComboRoll();
            UpdateTurn();
        }

    }
}
