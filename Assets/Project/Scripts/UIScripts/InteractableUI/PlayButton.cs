using System;
using Project.Scripts.UIScripts.Windows;
using UnityEngine;

namespace Project.Scripts.UIScripts.InteractableUI
{
    public class PlayButton : CustomButtonBase
    {
        public enum PlayButtonsFunctions
        {
            Play,
            Retry,
        }
        public PlayButtonsFunctions myFunction;
        private StarDisplayWindow myStarDisplayWindow;

        protected override void OnEnable()
        {
            base.OnEnable();
            myStarDisplayWindow ??= GetComponentInParent<StarDisplayWindow>();
        }

        protected override void Interact()
        {
            base.Interact();
            switch (myFunction)
            {
                case PlayButtonsFunctions.Play:
                    myStarDisplayWindow.Play();
                    break;
                case PlayButtonsFunctions.Retry:
                    MyMenuWindowHandler.RetryLevel();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
