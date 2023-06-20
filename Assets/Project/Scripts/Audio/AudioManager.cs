using System;
using Project.Scripts.General;
using Project.Scripts.Tiles;
using UnityEngine;

namespace Project.Scripts.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : Singleton<AudioManager>
    {
        [SerializeField] private AudioSource musicPlayer,audioSource1,audioSource2;
        private AudioClip win, lose, buttonClick;
        private AudioClip[] tileBreakSounds;
        private AudioClip partyClip, musicClip; // still need to define the sounds

        private bool partyMusicOn;
        
        protected override void Awake()
        {
            base.Awake();
            audioSource1 = gameObject.GetComponent<AudioSource>();
           
            buttonClick = Resources.Load<AudioClip>("Audio/SoundEffects/Button/click-21156");
        }

        public void StopMusic() => musicPlayer.Stop();
        public void ButtonClicked() => audioSource1.PlayOneShot(buttonClick);

        public void TileBreakSound(Tile.TileType tileType)
        {
            AudioClip clipToPlay = null;
            switch (tileType)
            {
                case Tile.TileType.Clear:
                    break;
                case Tile.TileType.EucalyptusTea: clipToPlay = tileBreakSounds[0];
                    break;
                case Tile.TileType.AppleTea:clipToPlay = tileBreakSounds[0];
                    break;
                case Tile.TileType.Mouse: clipToPlay = tileBreakSounds[1];
                    break;
                case Tile.TileType.Cookie: clipToPlay = tileBreakSounds[1];
                    break;
                case Tile.TileType.StrawberryCake: clipToPlay = tileBreakSounds[2];
                    break;
                case Tile.TileType.MoonCake: clipToPlay = tileBreakSounds[2];
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(tileType), tileType, null);
            }
            
            if(clipToPlay) audioSource1.PlayOneShot(clipToPlay);
        }

        public void TogglePartyMusic()
        {
            partyMusicOn = !partyMusicOn;
            musicPlayer.clip = partyMusicOn ? partyClip : musicClip;
            musicPlayer.Play();
        }
    }
}