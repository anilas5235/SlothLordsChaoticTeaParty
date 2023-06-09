using Project.Scripts.General;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project.Scripts.Menu
{
    public class SceneMaster : Singleton<SceneMaster>
    {
        public void ChangeToMenuScene()
        {
            CursorManager.Instance.DeActivateCursor();
            SceneManager.LoadScene("Menu");
        }

        public void ChangeToLevel(int levelID)
        {
            CursorManager.Instance.ActivateCursor();
            PlayerPrefs.SetInt("levelID", levelID);
            SceneManager.LoadScene("Main");
        }
    }
}
