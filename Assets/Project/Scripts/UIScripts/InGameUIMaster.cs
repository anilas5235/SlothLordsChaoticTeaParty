using System;
using Project.Scripts.General;
using Project.Scripts.Tiles;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Project.Scripts.UIScripts
{
    public class InGameUIMaster : Singleton<InGameUIMaster>
    {

        [Header("WindowControllers")]
        [SerializeField] private GameObject pause;
        [SerializeField] private GameObject options,audioOptions,win,lose;
        private UIStates currentUIState;

        public enum UIStates
        {
            Normal = 0,
            Pause = 1,
            Options = 2,
            AudioOptions = 3,
            Lose = 5,
            Winn = 6,
        }

        private void Update()
        {
            if (Input.GetButtonDown("Cancel")) UIEsc();
        }

        public void ChangeUIState(UIStates newState)
        {
            switch (currentUIState)
            {
                case UIStates.Normal: Time.timeScale = 0;
                    TileFieldManager.instance.interactable = false;
                    break;
                case UIStates.Pause: pause.SetActive(false);
                    break;
                case UIStates.Options: options.SetActive(false);
                    break;
                case UIStates.AudioOptions: audioOptions.SetActive(false); 
                    break;
                case UIStates.Lose: lose.SetActive(false);
                    break;
                case UIStates.Winn: win.SetActive(false);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            currentUIState = newState;

            switch (currentUIState)
            {
                case UIStates.Normal: Time.timeScale = 1; TileFieldManager.instance.interactable = true;
                    break;
                case UIStates.Pause: pause.SetActive(true);
                    break;
                case UIStates.Options: options.SetActive(true);
                    break;
                case UIStates.AudioOptions: audioOptions.SetActive(true);
                    break;
                case UIStates.Lose: lose.SetActive(true); AudioManager.instance.StopMusic();
                    AudioManager.instance.PlayLoseSound();
                    break;
                case UIStates.Winn: win.SetActive(true); AudioManager.instance.StopMusic();
                    AudioManager.instance.PlayWinSound();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        

      /*  private void ChangeUIStateInTitle(UIStates newState)
        {
            switch (currentUIState)
            {
                case UIStates.Normal: windowControllers[0].SetActive(false);
                    break;
                case UIStates.Options: windowControllers[1].SetActive(false);
                    break;
                case UIStates.AudioOptions: windowControllers[2].SetActive(false);
                    break;
            }

            currentUIState = newState;

            switch (currentUIState)
            {
                case UIStates.Normal: windowControllers[0].SetActive(true);
                    break;
                case UIStates.Options: windowControllers[1].SetActive(true);
                    break;
                case UIStates.AudioOptions: windowControllers[2].SetActive(true);
                    break;
            }
        }*/


      public void ChangeUIStateWithIndex(int index)
      {
          var state = index switch
          {
              0 => UIStates.Normal,
              1 => UIStates.Pause,
              2 => UIStates.Options,
              3 => UIStates.AudioOptions,
              4 => UIStates.Lose,
              5 => UIStates.Winn,
              _ => throw new ArgumentException("No defined State for this Index")
          };
          ChangeUIState(state);
      }

      public void UIEsc()
      {
          UIStates state;

          switch (currentUIState)
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

      public void QuitApplication() => Application.Quit();
      public void ChangeScene(int id)=> SceneManager.LoadScene(id);
    }
}
