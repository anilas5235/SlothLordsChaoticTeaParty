using System;
using Project.Scripts.General;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Project.Scripts.UIScripts.Menu
{
    public class AudioOptionsUIManager : UIMenuController
    {
        [SerializeField] private Slider main, music, effects,voice;
        [SerializeField] private AudioMixer mainAudioMixer;

        private void OnEnable()
        {
            LoadFromSaveText();
        }

        private void OnDisable()
        {
            SaveOptionsToText();
        }

        private void OnApplicationQuit()
        {
            SaveOptionsToText();
        }

        public void UpdateSoundOptions()
        {
            mainAudioMixer.SetFloat("Master", ConvertSliderValueTodB(main.value));
            mainAudioMixer.SetFloat("Music", ConvertSliderValueTodB(music.value));
            mainAudioMixer.SetFloat("Effects", ConvertSliderValueTodB(effects.value));
            mainAudioMixer.SetFloat("Voice", ConvertSliderValueTodB(voice.value));
        }

        private void SaveOptionsToText()
        {
            mainAudioMixer.GetFloat("Master", out SaveSystem.instance.GetActiveSave().audioOptions[0]);
            mainAudioMixer.GetFloat("Music", out SaveSystem.instance.GetActiveSave().audioOptions[1]);
            mainAudioMixer.GetFloat("Effects", out SaveSystem.instance.GetActiveSave().audioOptions[2]);
            mainAudioMixer.GetFloat("Voice", out SaveSystem.instance.GetActiveSave().audioOptions[3]);
        }
        private void LoadFromSaveText()
        {
            float[] optionsValues = SaveSystem.instance.GetActiveSave().audioOptions;
            mainAudioMixer.SetFloat("Master", optionsValues[0]);
            mainAudioMixer.SetFloat("Music", optionsValues[1]);
            mainAudioMixer.SetFloat("Effects", optionsValues[2]);
            mainAudioMixer.SetFloat("Voice", optionsValues[3]);

            main.value = ConvertDBToSliderValue(optionsValues[0]);
            music.value = ConvertDBToSliderValue(optionsValues[1]);
            effects.value = ConvertDBToSliderValue(optionsValues[2]);
            voice.value = ConvertDBToSliderValue(optionsValues[3]);
        }
        
        private float ConvertSliderValueTodB(float sliderValue) { return Mathf.Log10(sliderValue) * 20f; }
        private float ConvertDBToSliderValue(float dBValue) { return Mathf.Pow(10,(dBValue) / 20f); }

    }
}
