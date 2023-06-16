using System.Collections;
using TMPro;
using UnityEngine;

namespace Project.Scripts.UIScripts.Effects
{
    public class ScorePopUp : MonoBehaviour
    {
        private TMP_Text myText;
        private int maximumFontSize;
        private bool fade;
        private void OnEnable()
        {
            myText ??= GetComponent<TMP_Text>();
        }
        
        public void PassValues(Color textColor, int maxFontSize, string text)
        {
            myText.color = textColor;
            maximumFontSize = maxFontSize;
            myText.fontSize = maximumFontSize;
            fade = true;
            myText.text = text;
            StartCoroutine(Grow());
        }

        private IEnumerator Grow()
        {
            float fadeTime = 1f;
            float count = 0;
            float fadeStepTime = .1f;
            float fadeConstant = fadeStepTime / fadeTime;
            while(fade)
            {
                if (count <= fadeTime)
                {
                    if(count>.5f) myText.color -= new Color(0, 0, 0, fadeConstant);
                    myText.fontSize = 10 + maximumFontSize*count;
                }
                else
                {
                    fade = false;
                    ScorePopUpPool.Instance.AddObjectToPool(gameObject);
                }

                count+=fadeConstant;
                yield return new WaitForSeconds(fadeStepTime);
            }
        }
    }
}
