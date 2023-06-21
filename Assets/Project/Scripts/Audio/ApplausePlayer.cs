using System;
using System.Collections;
using Project.Scripts.General;
using Project.Scripts.Tiles;
using UnityEngine;

namespace Project.Scripts.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class ApplausePlayer : Singleton<ApplausePlayer>
    {
        [SerializeField] private float fadeTime =2f;
        private float fadeStep;
        private AudioSource source;
        private AudioClip applause;
        [SerializeField] private bool effectPlays = false;
        private Coroutine fader;

        protected override void Awake()
        {
            source = GetComponent<AudioSource>();
            fadeStep = 1 / (fadeTime / Time.fixedDeltaTime);
            source.loop = true;
            applause = Resources.Load<AudioClip>("Audio/SoundEffects/Event/perfect_game_cheer");
        }

        private void Start()
        {
            TileFieldManager.Instance.OnCombo += StartEffect;
            TileFieldManager.Instance.OnDoneFalling += FadeOutApplause;
        }

        private void StartEffect(TileFieldManager.ComboAppraisal appraisal)
        {
            if (appraisal == TileFieldManager.ComboAppraisal.Party) FadeInApplause();
        }

        private void FadeInApplause()
        {
           
            if (!effectPlays)
            {
                if (fader != null) return;
                fader = StartCoroutine(FadeApplause(true));
            }
        }


        private void FadeOutApplause()
        {
            if (effectPlays)
            {
                if (fader != null) return;
                fader = StartCoroutine(FadeApplause(false));
            }
        }

        private IEnumerator FadeApplause(bool @in)
        {
            if (@in)
            {
                source.clip = applause;
                source.volume = 1f;
                source.Play();
                effectPlays = true;
            }
            else
            {
                source.volume = 1f;
                do
                {
                    source.volume -= fadeStep;
                    yield return new WaitForFixedUpdate();
                } while (source.volume>0f);

                source.volume = 0f;
                source.Stop();
                effectPlays = false;
            }

            fader = null;
        }
    }
}
