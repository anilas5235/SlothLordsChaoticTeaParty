using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.DialogScripts
{
    public class ChoiceButton : MonoBehaviour
    {
        [SerializeField] private int choiceID;
        private Button myButton;
        public TextMeshProUGUI buttonText;

        private void Awake()
        {
            myButton = GetComponent<Button>();
            buttonText = GetComponentInChildren<TextMeshProUGUI>();
            myButton.onClick.AddListener( Choose);
        }
        private void Choose()
        {
             DialogManager.Instance.Choose(choiceID);
        }
    }
}
