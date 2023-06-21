using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Project.Scripts.General;
using Project.Scripts.Menu;
using UnityEngine;

namespace Project.Scripts.UIScripts.Windows
{
    public class MenuWindowsMaster : Singleton<MenuWindowsMaster>
    {
        [SerializeField] private UIMenuWindowHandler menuWindowToOpen;
        [SerializeField] private bool DeactivateCursorOnMenuClose = true;

        public Action<bool> OnMenuActiveChange; 
        [SerializeField] private bool menuActive;

        private FirstPersonController playerController;

        protected override void Awake()
        {
            base.Awake();
            playerController = FindObjectOfType<FirstPersonController>();
        }

        private void Start()
        {
            AudioOptionsUIMenuWindow audioOptions = FindObjectOfType<AudioOptionsUIMenuWindow>(true);
            if (audioOptions)
            {
                audioOptions.LoadFromSaveText();
            }
        }

        public List<UIMenuWindowHandler> currentlyActiveWindows = new List<UIMenuWindowHandler>();

     

        public bool MenuActive
        {
            get => menuActive;
            set
            {
                if (!value == menuActive)
                {
                    menuActive = value;
                    OnMenuActiveChange?.Invoke(menuActive);
                }
            }
        }


        private void Update()
        {
            if (Input.GetButtonDown("Cancel"))
            {
                if (!MenuActive) OpenWindow(); else currentlyActiveWindows.Last().UIEsc();
            }
        }

        public void OpenWindow(UIMenuWindowHandler windowToOpen = null)
        {
            if (windowToOpen == null) menuWindowToOpen.ActivateWindow();
            else windowToOpen.ActivateWindow();
          
            MenuActive = true;
            CursorManager.Instance.ActivateCursor();
            if (playerController != null) playerController.FreezePlayer();
        }

        public void UpdateState()
        {
            StartCoroutine(UpdateMenuState());
        }
        
        private IEnumerator UpdateMenuState()
        {
            yield return new WaitForEndOfFrame();
            if (!currentlyActiveWindows.Any())
            {
                MenuActive = false;
                if(DeactivateCursorOnMenuClose) CursorManager.Instance.DeActivateCursor();
                if (playerController != null) playerController.UnFreezePlayer();
            }
        }
    }
}
