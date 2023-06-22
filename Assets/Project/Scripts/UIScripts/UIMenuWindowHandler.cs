using System;
using Project.Scripts.UIScripts.Windows;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project.Scripts.UIScripts
{
    public class UIMenuWindowHandler : MonoBehaviour
    {
        [SerializeField] protected bool interactable = true;
        public bool Interactable
        {
            get => interactable;
            protected set
            {
                interactable = value;
                windowInteractabilityUpdate?.Invoke(interactable);
            }
        }
        
        [SerializeField] protected UIMenuWindowHandler parentMenuWindow;
        private MenuWindowsMaster myWindowsMaster;
        
        public Action<bool> windowInteractabilityUpdate;

        public enum StandardUIButtonFunctions
        {
            Esc,
            ChangeWindow,
            OpenWindow,
            Quit,
            ChangeScene,
            MainMenu,
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
        
        public void ChangeScene(string name) => SceneManager.LoadScene(name);

        public virtual void ActivateWindow()
        {
            gameObject.SetActive(true);
            myWindowsMaster ??= MenuWindowsMaster.Instance;
            myWindowsMaster.currentlyActiveWindows.Add(this);
        }

        public virtual void SwitchToMainMenu()
        {
            ChangeScene("Menu");
        }

        public virtual void RetryLevel()
        {
            ChangeScene("Main");
        }
    }
}
