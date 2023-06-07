using Project.Scripts.General;
using UnityEngine;

namespace Project.Scripts.UIScripts
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : Singleton<AudioManager>
    {
        [SerializeField] private AudioSource musicPlayer,audioSource1,audioSource2;
        private AudioClip win, lose, buttonClick;
        
        protected override void Awake()
        {
            base.Awake();
            audioSource1 = gameObject.GetComponent<AudioSource>();
           
            buttonClick = Resources.Load<AudioClip>("Audio/SoundEffects/Button/click-21156");
            win = Resources.Load<AudioClip>("Audio/SoundEffects/Event/success-fanfare-trumpets-6185");
            lose  = Resources.Load<AudioClip>("Audio/SoundEffects/Event/failure-1-89170");
        }

        public void StopMusic() => musicPlayer.Stop();
        public void ButtonClicked() => audioSource1.PlayOneShot(buttonClick);

        public void PlayWinSound() => audioSource1.PlayOneShot(win);
        public void PlayLoseSound() => audioSource1.PlayOneShot(lose);
    }
}