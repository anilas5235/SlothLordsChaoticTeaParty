using Project.Scripts.UIScripts.Windows;

namespace Project.Scripts.UIScripts.InteractableUI
{
    public class AudioSlider : CustomSliderBase
    {
        private AudioOptionsUIMenuWindow audioMenuWindow;
        protected override void OnEnable()
        {
            base.OnEnable();
            audioMenuWindow ??=  GetComponentInParent<AudioOptionsUIMenuWindow>();
        }

        protected override void SliderChanged(float val)
        {
            if(!gameObject.activeInHierarchy) return;
            audioMenuWindow.UpdateSoundOptions();
        }
    }
}
