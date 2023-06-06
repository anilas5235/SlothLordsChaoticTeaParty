using UnityEngine;

namespace Project.Scripts.UIScripts.Menu
{
    public class UIMenuController : MonoBehaviour
    {
        private void ButtonClicked()
        {
            AudioManager.instance.ButtonClicked();
        }
        
        public void Esc()
        {
            ButtonClicked();
            InGameUIMaster.instance.UIEsc();
        }

        public void ChangeScene(int index)
        {
            ButtonClicked();
            InGameUIMaster.instance.ChangeScene(index);
        }

        public void OpenOptionsMenu()
        {
            ButtonClicked();
            InGameUIMaster.instance.ChangeUIState(InGameUIMaster.UIStates.Options);
        }

        public void OpenAudioOptionsMenu()
        {
            ButtonClicked();
            InGameUIMaster.instance.ChangeUIState(InGameUIMaster.UIStates.AudioOptions);
        }

        public void ResumeGame()
        {
            ButtonClicked();
            InGameUIMaster.instance.ChangeUIState(InGameUIMaster.UIStates.Normal);
        }

        public void Quit()
        {
            ButtonClicked();
            InGameUIMaster.instance.QuitApplication();
        }
    }
}