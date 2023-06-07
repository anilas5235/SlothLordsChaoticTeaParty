using Project.Scripts.UIScripts.Menu;

namespace Project.Scripts.UIScripts.InteractableUI
{
    public class AudioSlider : CustomSliderBase
    {
        private AudioOptionsUIWindow audioWindow;
        protected override void OnEnable()
        {
            base.OnEnable();
            audioWindow ??=  GetComponentInParent<AudioOptionsUIWindow>();
        }

        protected override void SliderChanged(float val)
        {
            audioWindow.UpdateSoundOptions();
        }
    }
}
