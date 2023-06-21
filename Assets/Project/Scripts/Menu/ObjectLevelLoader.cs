using System;
using Project.Scripts.General;
using Project.Scripts.UIScripts.Windows;
using UnityEngine;

namespace Project.Scripts.Menu
{
    public class ObjectLevelLoader : InteractableObject

    {
        [SerializeField] private int levelID;
        [SerializeField] private bool constantHighlight;

        private bool unlocked;

        private void Start()
        {
            unlocked =SaveSystem.Instance.GetActiveSave().levelsUnlocked[levelID];
            myOutline.enabled = unlocked;
            if (levelID == 0 && !SaveSystem.Instance.GetActiveSave().firstTimeTutorialDone)
            {
                constantHighlight = true;
            }
        }

        private void FixedUpdate()
        {
            if (constantHighlight && myOutline.eraseRenderer) myOutline.eraseRenderer = false;
        }

        public override void Interact()
        {
            if(!unlocked || MenuWindowsMaster.Instance.MenuActive)return;
            
            base.Interact();
            
            PlayPreviewWindow.Instance.SetLeveVar(levelID);
            MenuWindowsMaster.Instance.OpenWindow(PlayPreviewWindow.Instance);
        }
    }
}
