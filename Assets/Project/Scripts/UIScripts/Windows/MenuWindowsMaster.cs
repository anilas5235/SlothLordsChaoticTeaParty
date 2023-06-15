using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Project.Scripts.General;
using Project.Scripts.Menu;
using UnityEngine;

namespace Project.Scripts.UIScripts.Menu
{
    public class MenuWindowsMaster : Singleton<MenuWindowsMaster>
    {
        [SerializeField] private UIMenuWindowHandler menuWindowToOpen;

        public Action<bool> OnMenuActiveChange;

        private FirstPersonController playerController;

        protected override void Awake()
        {
            base.Awake();
            playerController = FindObjectOfType<FirstPersonController>();
        }

        public List<UIMenuWindowHandler> currentlyActiveWindows = new List<UIMenuWindowHandler>();

        [SerializeField] private bool menuActive;

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
                CursorManager.Instance.DeActivateCursor();
                if (playerController != null) playerController.UnFreezePlayer();
            }
        }
    }
}
