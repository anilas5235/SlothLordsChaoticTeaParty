using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Project.Scripts.General;
using Project.Scripts.Menu;
using UnityEngine;

namespace Project.Scripts.UIScripts.Menu
{
    public class MenuWindowsMaster : MonoBehaviour
    {
        [SerializeField] private UIMenuWindowHandler menuWindowToOpen;

        private FirstPersonController playerController;

        private void Awake()
        {
            playerController = FindObjectOfType<FirstPersonController>();
        }

        public List<UIMenuWindowHandler> CurrentlyActiveWindows;

        public bool menuActive;

        private void Update()
        {
            if (Input.GetButtonDown("Cancel"))
            {
                if (!menuActive)
                {
                    OpenWindow();
                }
                else
                {
                    CurrentlyActiveWindows.Last().UIEsc();
                }
            }
            
        }

        private void OpenWindow()
        {
            menuWindowToOpen.ActivateWindow(this);
            menuActive = true;
            CursorManager.Instance.ActivateCursor();
            if (playerController != null) playerController.FreezePlayerToggle();
        }

        public void UpdateState()
        {
            StartCoroutine(UpdateMenuState());
        }
        
        private IEnumerator UpdateMenuState()
        {
            yield return new WaitForEndOfFrame();
            if (!CurrentlyActiveWindows.Any())
            {
                menuActive = false;
                CursorManager.Instance.DeActivateCursor();
                if (playerController != null) playerController.FreezePlayerToggle();
            }
        }
    }
}
