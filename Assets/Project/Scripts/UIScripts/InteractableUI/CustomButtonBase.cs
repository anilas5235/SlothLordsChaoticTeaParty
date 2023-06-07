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
        }

        protected virtual void OnDisable()
        {
            myInteractable.onClick.RemoveListener(Interact);
        }

        protected override void Interact()
        {
            AudioManager.instance.ButtonClicked();
        }
    }
}
