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
                    MyMenuWindowHandler.UIEsc();
                    break;
                case UIMenuWindowHandler.StandardUIButtonFunctions.ChangeWindow:
                    MyMenuWindowHandler.ChangeToWindow(menuWindowHandler);
                    break;
                case UIMenuWindowHandler.StandardUIButtonFunctions.OpenWindow:
                    MyMenuWindowHandler.OpenWindow(menuWindowHandler);
                    break;
                case UIMenuWindowHandler.StandardUIButtonFunctions.Quit:
                    MyMenuWindowHandler.QuitApplication();
                    break;
                case UIMenuWindowHandler.StandardUIButtonFunctions.ChangeScene:
                    MyMenuWindowHandler.ChangeScene(sceneID);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
