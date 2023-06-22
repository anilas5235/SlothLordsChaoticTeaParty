using System;
using Project.Scripts.UIScripts.Windows;
using UnityEngine;

namespace Project.Scripts.UIScripts.InteractableUI
{
    public class PlayButton : CustomButtonBase
    {
        public PlayButtonsFunctions myFunction;
        
        private StarDisplayWindow myStarDisplayWindow;

        public enum PlayButtonsFunctions
        {
            Play,
            Retry,
            EndDialog
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            myStarDisplayWindow ??= GetComponentInParent<StarDisplayWindow>();
        }

        protected override void Interact()
        {
            if(!myStarDisplayWindow.Interactable) return;
            base.Interact();
            switch (myFunction)
            {
                case PlayButtonsFunctions.Play:
                    myStarDisplayWindow.Play();
                    break;
                case PlayButtonsFunctions.Retry:
                    myMenuWindowHandler.RetryLevel();
                    break;
                case PlayButtonsFunctions.EndDialog:
                    myStarDisplayWindow.PlayEndDialog();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
