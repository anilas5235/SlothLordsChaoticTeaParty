using UnityEngine;

namespace Project.Scripts.UIScripts.InteractableUI
{
    public abstract class CustomUIInteractableBase<T> : MonoBehaviour
    {
        protected T myInteractable;
        protected UIMenuWindowHandler myMenuWindowHandler;

        protected virtual void OnEnable()
        {
            myInteractable ??= GetComponent<T>();
            myMenuWindowHandler ??= GetComponentInParent<UIMenuWindowHandler>();
        }

        protected virtual void Interact(){}
    }
}
