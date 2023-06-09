using UnityEngine;

namespace Project.Scripts.UIScripts.Menu
{
    public class UIMenuController : MonoBehaviour
    {
        private void ButtonClicked()
        {
            AudioManager.Instance.ButtonClicked();
        }
        
        public void Esc()
        {
            ButtonClicked();
            InGameUIMaster.Instance.UIEsc();
        }

        public void ChangeScene(int index)
        {
            ButtonClicked();
            InGameUIMaster.Instance.ChangeScene(index);
        }

        public void OpenOptionsMenu()
        {
            ButtonClicked();
            InGameUIMaster.Instance.ChangeUIState(InGameUIMaster.UIStates.Options);
        }

        public void OpenAudioOptionsMenu()
        {
            ButtonClicked();
            InGameUIMaster.Instance.ChangeUIState(InGameUIMaster.UIStates.AudioOptions);
        }

        public void ResumeGame()
        {
            ButtonClicked();
            InGameUIMaster.Instance.ChangeUIState(InGameUIMaster.UIStates.Normal);
        }

        public void Quit()
        {
            ButtonClicked();
            InGameUIMaster.Instance.QuitApplication();
        }
    }
}