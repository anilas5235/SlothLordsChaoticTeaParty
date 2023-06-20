using Project.Scripts.General;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project.Scripts.Menu
{
    public class SceneMaster : Singleton<SceneMaster>
    {
        public void ChangeToMenuScene()
        {
            if(CursorManager.Instance) CursorManager.Instance.DeActivateCursor();
            SceneManager.LoadScene("Menu");
        }

        public void ChangeToLevelDialog(int levelID, int dialogID)
        {
            CursorManager.Instance.ActivateCursor();
            PlayerPrefs.SetInt("levelID", levelID);
            PlayerPrefs.SetInt("DialogID",dialogID);
            SceneManager.LoadScene("Dialog");
        }

        public void LoadLevel()
        {
            CursorManager.Instance.ActivateCursor();
            SceneManager.LoadScene(PlayerPrefs.GetInt("levelID", 0) == 0 ? "Tutorial" : "Main");
        }
    }
}
