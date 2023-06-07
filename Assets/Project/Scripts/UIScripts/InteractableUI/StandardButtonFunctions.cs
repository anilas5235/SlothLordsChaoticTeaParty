using System;

namespace Project.Scripts.UIScripts.InteractableUI
{
    public class StandardButtonFunctions : CustomButtonBase
    {
        public UIWindowHandler.StandardUIButtonFunctions myFunction;

        public UIWindowHandler windowHandler;
        public int sceneID;

        protected override void Interact()
        {
            base.Interact();
            switch (myFunction)
            {
                case UIWindowHandler.StandardUIButtonFunctions.Esc:
                    myWindowHandler.UIEsc();
                    break;
                case UIWindowHandler.StandardUIButtonFunctions.ChangeWindow:
                    myWindowHandler.ChangeToWindow(windowHandler);
                    break;
                case UIWindowHandler.StandardUIButtonFunctions.OpenWindow:
                    myWindowHandler.OpenWindow(windowHandler);
                    break;
                case UIWindowHandler.StandardUIButtonFunctions.Quit:
                    myWindowHandler.QuitApplication();
                    break;
                case UIWindowHandler.StandardUIButtonFunctions.ChangeScene:
                    myWindowHandler.ChangeScene(sceneID);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
