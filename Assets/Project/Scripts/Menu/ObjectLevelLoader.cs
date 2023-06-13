using Project.Scripts.General;
using Project.Scripts.UIScripts;
using Project.Scripts.UIScripts.Menu;
using UnityEngine;

namespace Project.Scripts.Menu
{
    public class ObjectLevelLoader : InteractableObject

    {
        [SerializeField] private int levelID;

        private bool unlocked;

        private void Start()
        {
            unlocked =SaveSystem.Instance.GetActiveSave().levelsUnlocked[levelID];
            myOutline.enabled = unlocked;
        }

        public override void Interact()
        {
            if(!unlocked)return;
            MenuWindowsMaster.Instance.OpenWindow(PlayPreviewWindow.Instance);
            PlayPreviewWindow.Instance.levelID = levelID;
        }
    }
}
