using Project.Scripts.Audio;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.UIScripts.InteractableUI
{
    [RequireComponent(typeof(Button))]
    public abstract class CustomButtonBase : CustomUIInteractableBase<Button>
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            myInteractable.onClick.AddListener(Interact);
            if (myMenuWindowHandler) myMenuWindowHandler.windowInteractabilityUpdate += WindowIntractabilityChanged;
        }

        protected virtual void OnDisable()
        {
            myInteractable.onClick.RemoveListener(Interact);
        }

        protected virtual void WindowIntractabilityChanged(bool newVal) => myInteractable.interactable = newVal;

        protected override void Interact()
        {
            AudioManager.Instance.ButtonClicked();
        }
    }
}
