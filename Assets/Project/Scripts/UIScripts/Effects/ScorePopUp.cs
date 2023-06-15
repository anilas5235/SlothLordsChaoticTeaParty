using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Project.Scripts.UIScripts.Effects
{
    public class ScorePopUp : MonoBehaviour
    {
        private TMP_Text myText;
        private int maximumFontSize;
        private bool grow;
        private void OnEnable()
        {
            myText ??= GetComponent<TMP_Text>();
        }
        
        public void PassValues(Color textColor, int maxFontSize, string text)
        {
            myText.color = textColor+ new Color(0, 0, 0, 1);
            maximumFontSize = maxFontSize;
            myText.fontSize = 10;
            grow = true;
            myText.text = text;
            StartCoroutine(Grow());
        }

        private IEnumerator Grow()
        {
            while(grow)
            {
                if (myText.fontSize < maximumFontSize) myText.fontSize+=maximumFontSize/20f;
                else if (myText.color.a > 0) myText.color -= new Color(0, 0, 0, .1f);
                else
                {
                    grow = false;
                    ScorePopUpPool.Instance.AddObjectToPool(gameObject);
                }

                yield return new WaitForSeconds(.1f);
            }
        }
    }
}
