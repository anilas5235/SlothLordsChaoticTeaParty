using System.Collections;
using Project.Scripts.General;
using UnityEngine;

//Code from Shawn Okorie

namespace Project.Scripts.DialogScripts
{
    public class ScreenFade : Singleton<ScreenFade>
    {
        private CanvasGroup blackScreen;

        public float waitDuration;
        public float fadeDuration;
        protected override void Awake()
        {
            base.Awake();
            blackScreen = GetComponent<CanvasGroup>();
        }
    

        public void StartFadeIn()
        {
            gameObject.SetActive(true);
            StartCoroutine(FadeIn(waitDuration, fadeDuration));
        }
    
        public void StartFadeOut()
        {
            gameObject.SetActive(true);
            StartCoroutine(FadeOut(waitDuration, fadeDuration));
        }
    
        private IEnumerator FadeIn(float waitDuration, float fadeDuration)
        {
            OnFadeStart?.Invoke();
            myCanvas.sortingOrder = 10;
            blackScreen.alpha = 1;
        
            yield return new WaitForSeconds(waitDuration);
        
            for (float t = 0; t < fadeDuration; t += Time.deltaTime)
            {
                blackScreen.alpha = Mathf.Lerp(1,0,t / fadeDuration);
            
                yield return null;
            }
        
            blackScreen.alpha = 0;
            gameObject.SetActive(false);
            OnFadeEnd?.Invoke();
        }
    
        private IEnumerator FadeOut(float waitDuration, float fadeDuration)
        {
            OnFadeStart?.Invoke();
            blackScreen.alpha = 0;
            myCanvas.sortingOrder = 10;
        
            yield return new WaitForSeconds(waitDuration);
        
            for (float t = 0; t < fadeDuration; t += Time.deltaTime)
            {
                blackScreen.alpha = Mathf.Lerp(0,1,t / fadeDuration);
            
                yield return null;
            }
            blackScreen.alpha = 1;
            gameObject.SetActive(false);
            OnFadeEnd?.Invoke();
        }
    }
}
