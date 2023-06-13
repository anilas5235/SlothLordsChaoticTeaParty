using Project.Scripts.UIScripts;
using Project.Scripts.UIScripts.Menu;
using UnityEngine;

namespace Project.Scripts.Menu
{
    public class ObjectLevelLoader : InteractableObject

    {
        [SerializeField] private int levelID;

        public override void Interact()
        {
            MenuWindowsMaster.Instance.OpenWindow(PlayPreviewWindow.Instance);
            PlayPreviewWindow.Instance.levelID = levelID;
        }
    }
}
