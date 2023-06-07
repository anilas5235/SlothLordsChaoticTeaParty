using Project.Scripts.General;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project.Scripts.UIScripts
{
    public abstract class BaseUIMaster : Singleton<BaseUIMaster>
    {
        protected UIStates CurrentUIState;

        public enum UIStates
        {
            Normal = 0,
            Pause = 1,
            Options = 2,
            AudioOptions = 3,
            Lose = 5,
            Winn = 6,
        }

        protected virtual void Update()
        {
            if (Input.GetButtonDown("Cancel")) UIEsc();
        }

        public abstract void ChangeUIState(UIStates newState);

        public abstract void UIEsc();

        public void QuitApplication() => Application.Quit();
        public void ChangeScene(int id) => SceneManager.LoadScene(id);
    }
}
