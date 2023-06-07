using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project.Scripts.UIScripts
{
    public class UIWindowHandler : MonoBehaviour
    {
        [SerializeField] protected UIWindowHandler parentWindow;
        
        public enum StandardUIButtonFunctions
        {
            Esc,
            ChangeWindow,
            OpenWindow,
            Quit,
            ChangeScene,
        }
        public virtual void UIEsc()
        {
            if (parentWindow != null) parentWindow.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }

        public virtual void ChangeToWindow(UIWindowHandler windowMaster)
        {
            windowMaster.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }

        public virtual void OpenWindow(UIWindowHandler windowMaster)
        {
            windowMaster.gameObject.SetActive(true);
        }

        public void QuitApplication() => Application.Quit();
        public void ChangeScene(int id) => SceneManager.LoadScene(id);
    }
}
