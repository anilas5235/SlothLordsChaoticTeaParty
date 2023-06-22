using System;

namespace Project.Scripts.UIScripts.InteractableUI
{
    public class StandardButtonFunctions : CustomButtonBase
    {
        public UIMenuWindowHandler.StandardUIButtonFunctions myFunction;

        public UIMenuWindowHandler menuWindowHandler;
        public int sceneID;

        protected override void Interact()
        {
            base.Interact();
            switch (myFunction)
            {
                case UIMenuWindowHandler.StandardUIButtonFunctions.Esc:
                    myMenuWindowHandler.UIEsc();
                    break;
                case UIMenuWindowHandler.StandardUIButtonFunctions.ChangeWindow:
                    myMenuWindowHandler.ChangeToWindow(menuWindowHandler);
                    break;
                case UIMenuWindowHandler.StandardUIButtonFunctions.OpenWindow:
                    myMenuWindowHandler.OpenWindow(menuWindowHandler);
                    break;
                case UIMenuWindowHandler.StandardUIButtonFunctions.Quit:
                    myMenuWindowHandler.QuitApplication();
                    break;
                case UIMenuWindowHandler.StandardUIButtonFunctions.ChangeScene:
                    myMenuWindowHandler.ChangeScene(sceneID);
                    break;

                case UIMenuWindowHandler.StandardUIButtonFunctions.MainMenu:
                    myMenuWindowHandler.SwitchToMainMenu();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
