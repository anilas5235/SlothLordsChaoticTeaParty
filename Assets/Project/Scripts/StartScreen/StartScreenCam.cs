using System;
using System.Collections;
using Project.Scripts.DialogScripts;
using Project.Scripts.Menu;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.StartScreen
{
    public class StartScreenCam : MonoBehaviour
    {
        [SerializeField] private Canvas titleImage;
            
        private Animation myAnimation;

        private void Awake()
        {
            myAnimation = GetComponent<Animation>();
        }

        private void Update()
        {
            if (Input.GetButtonDown("Jump"))
            {
                titleImage.gameObject.SetActive(false);
                myAnimation.Play();
                StartCoroutine(ToMenuAfter(myAnimation.clip.length));
                ScreenFade.Instance.StartFadeOut(myAnimation.clip.length - 1f);
            }
        }

        private IEnumerator ToMenuAfter(float time)
        {
            yield return new WaitForSeconds(time);
            SceneMaster.Instance.ChangeToMenuScene();
        }
    }
}
