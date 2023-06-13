using System;
using Project.Scripts.UIScripts.Menu;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project.Scripts.UIScripts
{
    public class UIMenuWindowHandler : MonoBehaviour
    {
        [SerializeField] protected UIMenuWindowHandler parentMenuWindow;
        private MenuWindowsMaster myWindowsMaster;

        
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
            if (parentMenuWindow != null) parentMenuWindow.ActivateWindow();
            myWindowsMaster.currentlyActiveWindows.Remove(this);
            myWindowsMaster.UpdateState();
            gameObject.SetActive(false);
        }

        public virtual void ChangeToWindow(UIMenuWindowHandler menuWindowMaster)
        {
            menuWindowMaster.ActivateWindow();
            myWindowsMaster.currentlyActiveWindows.Remove(this);
            myWindowsMaster.UpdateState();
            gameObject.SetActive(false);
        }

        public virtual void OpenWindow(UIMenuWindowHandler menuWindowMaster)
        {
            menuWindowMaster.ActivateWindow();
        }

        public void QuitApplication() => Application.Quit();
        public void ChangeScene(int id) => SceneManager.LoadScene(id);

        public void ActivateWindow()
        {
            gameObject.SetActive(true);
            myWindowsMaster ??= MenuWindowsMaster.Instance;
            myWindowsMaster.currentlyActiveWindows.Add(this);
        }
    }
}
