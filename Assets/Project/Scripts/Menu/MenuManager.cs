using System;
using Project.Scripts.General;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project.Scripts.Menu
{
    public class MenuManager : Singleton<MenuManager>
    {
        public enum Scenes
        {
            Tutorial,
            Level1,
            Level2,
            Level3,
        }


        public void LoadScene(Scenes sceneToLoad)
        {
            SceneManager.LoadScene(sceneToLoad.ToString());
        }
    }
}
