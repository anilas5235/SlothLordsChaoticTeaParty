using Project.Scripts.General;
using Project.Scripts.Tiles;
using TMPro;
using UnityEngine;

namespace Project.Scripts.UIScripts
{
    public class UIManager : Singleton<UIManager>
    {
        [SerializeField] private TextMeshProUGUI scoreText, comboRollText, turnText ;

        private void Start()
        {
            UpdateAllUIFields();
        }

        public void UpdateScore()
        {
            scoreText.text = $"Score: {TileManager.instance.Score}";
        }

        public void UpdateComboRoll()
        {
            comboRollText.text = $"Combos: {TileManager.instance.ComboRoll}";
        }

        public void UpdateTurn()
        {
            turnText.text = $"Turns: {TileManager.instance.turns}";
        }

        public void UpdateAllUIFields()
        {
            UpdateScore();
            UpdateComboRoll();
            UpdateTurn();
        }

    }
}
