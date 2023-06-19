using System;
using System.Collections;
using Project.Scripts.General;
using UnityEngine;

//Code from Shawn Okorie

namespace Project.Scripts.DialogScripts
{
    public class ScreenFade : Singleton<ScreenFade>
    {
        private CanvasGroup blackScreen;

        public float FadeDuration { get; private set; } = 3;
        protected override void Awake()
        {
            base.Awake();
            blackScreen = GetComponent<CanvasGroup>();
        }

        private void Start()
        {
            StartFadeIn();
        }

        public void StartFadeIn()
        {
            gameObject.SetActive(true);
            StartCoroutine(FadeIn( FadeDuration));
        }
    
        public void StartFadeOut()
        {
            gameObject.SetActive(true);
            StartCoroutine(FadeOut( FadeDuration));
        }
    
        private IEnumerator FadeIn(float fadeDuration)
        {
            blackScreen.alpha = 1;

            float fadeStep = 1/( fadeDuration/Time.fixedDeltaTime);

            while (blackScreen.alpha >0)
            {
                blackScreen.alpha -= fadeStep;

                yield return new WaitForFixedUpdate();
            }

            blackScreen.alpha = 0;
            gameObject.SetActive(false);
        }
    
        private IEnumerator FadeOut( float fadeDuration)
        {
            blackScreen.alpha = 0;

            float fadeStep = 1/( fadeDuration/Time.fixedDeltaTime);

            while (blackScreen.alpha <1)
            {
                blackScreen.alpha += fadeStep;

                yield return new WaitForFixedUpdate();
            }

            blackScreen.alpha = 1;
        }
    }
}
