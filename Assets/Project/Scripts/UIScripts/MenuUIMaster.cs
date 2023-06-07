using Project.Scripts.General;
using Project.Scripts.Menu;
using UnityEngine;

namespace Project.Scripts.UIScripts
{
    public class MenuUIMaster : BaseUIMaster
    {
        [Header("WindowControllers")] 
        [SerializeField] private GameObject pause;
        [SerializeField] private GameObject options, audioOptions;

        private FirstPersonController playerContoller;

        protected override void Awake()
        {
            base.Awake();
            playerContoller = FindObjectOfType<FirstPersonController>();
        }

        public override void ChangeUIState(UIStates newState)
        {
            switch (CurrentUIState)
            {
                case UIStates.Normal:playerContoller.FreezePlayerToggle(); CursorManager.instance.ActivateCursor();
                    break;
                case UIStates.Pause: pause.SetActive(false);
                    break;
                case UIStates.Options:
                    options.SetActive(false);
                    break;
                case UIStates.AudioOptions:
                    audioOptions.SetActive(false);
                    break;
            }

            CurrentUIState = newState;

            switch (CurrentUIState)
            {
                case UIStates.Normal: playerContoller.FreezePlayerToggle();CursorManager.instance.DeActivateCursor();
                    break;
                case UIStates.Pause: pause.SetActive(true);break;
                case UIStates.Options:
                    options.SetActive(true);
                    break;
                case UIStates.AudioOptions:
                    audioOptions.SetActive(true);
                    break;
            }

        }

        public override void UIEsc()
        {
            UIStates state;

            switch (CurrentUIState)
            {
                case UIStates.Normal:
                    state = UIStates.Pause;
                    break;
                case UIStates.Pause:
                    state = UIStates.Normal;
                    break;
                case UIStates.Options:
                    state = UIStates.Pause;
                    break;
                case UIStates.AudioOptions:
                    state = UIStates.Options;
                    break;
                default: return;
            }

            ChangeUIState(state);
        }
    }
}
