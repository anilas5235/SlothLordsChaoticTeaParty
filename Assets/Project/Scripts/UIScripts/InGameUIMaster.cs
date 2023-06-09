using System;
using Project.Scripts.Tiles;
using UnityEngine;

namespace Project.Scripts.UIScripts
{
    public class InGameUIMaster : BaseUIMaster
    {

        [Header("WindowControllers")] [SerializeField]
        private GameObject pause;
        [SerializeField] private GameObject options, audioOptions, win, lose;

        public override void ChangeUIState(UIStates newState)
        {
            switch (CurrentUIState)
            {
                case UIStates.Normal:
                    Time.timeScale = 0;
                    TileFieldManager.Instance.interactable = false;
                    break;
                case UIStates.Pause:
                    pause.SetActive(false);
                    break;
                case UIStates.Options:
                    options.SetActive(false);
                    break;
                case UIStates.AudioOptions:
                    audioOptions.SetActive(false);
                    break;
                case UIStates.Lose:
                    lose.SetActive(false);
                    break;
                case UIStates.Winn:
                    win.SetActive(false);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            CurrentUIState = newState;

            switch (CurrentUIState)
            {
                case UIStates.Normal:
                    Time.timeScale = 1;
                    TileFieldManager.Instance.interactable = true;
                    break;
                case UIStates.Pause:
                    pause.SetActive(true);
                    break;
                case UIStates.Options:
                    options.SetActive(true);
                    break;
                case UIStates.AudioOptions:
                    audioOptions.SetActive(true);
                    break;
                case UIStates.Lose:
                    lose.SetActive(true);
                    AudioManager.Instance.StopMusic();
                    AudioManager.Instance.PlayLoseSound();
                    break;
                case UIStates.Winn:
                    win.SetActive(true);
                    AudioManager.Instance.StopMusic();
                    AudioManager.Instance.PlayWinSound();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
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
