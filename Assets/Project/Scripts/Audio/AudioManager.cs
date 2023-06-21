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
        private AudioClip winGood,winPerfect, lose, buttonClick,switchTileSound;
        private AudioClip[] tileBreakSounds,comboSounds;


        protected override void Awake()
        {
            base.Awake();
            audioSource1 = gameObject.GetComponent<AudioSource>();
           
            buttonClick = Resources.Load<AudioClip>("Audio/SoundEffects/Button/click-21156");
            tileBreakSounds = new[]
            {
                Resources.Load<AudioClip>("Audio/SoundEffects/Break/break_02"),
                Resources.Load<AudioClip>("Audio/SoundEffects/Break/break_04"),
            };
            comboSounds = new []
            {
                Resources.Load<AudioClip>("Audio/SoundEffects/Combo/combo_pling"),
                Resources.Load<AudioClip>("Audio/SoundEffects/Combo/combo_pling_02")
            };

            lose = Resources.Load<AudioClip>("Audio/SoundEffects/Event/loosing_sound");
            winGood = Resources.Load<AudioClip>("Audio/SoundEffects/Event/good_game_cheer");
            winPerfect =Resources.Load<AudioClip>("Audio/SoundEffects/Event/perfect_game_cheer");
            switchTileSound = Resources.Load<AudioClip>("Audio/SoundEffects/Break/switch_items");
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
                case Tile.TileType.EucalyptusTea: clipToPlay = tileBreakSounds[1];
                    break;
                case Tile.TileType.AppleTea:clipToPlay = tileBreakSounds[1];
                    break;
                case Tile.TileType.Mouse: clipToPlay = tileBreakSounds[0];
                    break;
                case Tile.TileType.Cookie: clipToPlay = tileBreakSounds[0];
                    break;
                case Tile.TileType.StrawberryCake: clipToPlay = tileBreakSounds[0];
                    break;
                case Tile.TileType.MoonCake: clipToPlay = tileBreakSounds[0];
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(tileType), tileType, null);
            }
            
            if(clipToPlay) audioSource1.PlayOneShot(clipToPlay);
        }

        public void ComboSound(int comboSize)
        {
            audioSource1.PlayOneShot(comboSize > 4 ? comboSounds[1] : comboSounds[0]);
        }

        public void TriggerEndSound(int numberOfStars)
        {
            AudioClip clip = numberOfStars switch
            {
                0 => lose,
                1 => winGood,
                _ => winPerfect,
            };
            if(clip) audioSource1.PlayOneShot(clip);
        }

        public void PlayEffectClip(AudioClip clip)
        {
            audioSource1.PlayOneShot(clip);
        }

        public void PlayTileSwitch()
        {
            audioSource1.PlayOneShot(switchTileSound);
        }
    }
}