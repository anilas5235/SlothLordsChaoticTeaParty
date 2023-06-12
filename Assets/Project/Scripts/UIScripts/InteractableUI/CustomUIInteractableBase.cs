using UnityEngine;

namespace Project.Scripts.UIScripts.InteractableUI
{
    public abstract class CustomUIInteractableBase<T> : MonoBehaviour
    {
        protected T myInteractable;
        protected UIMenuWindowHandler MyMenuWindowHandler;

        protected virtual void OnEnable()
        {
            myInteractable ??= GetComponent<T>();
            MyMenuWindowHandler ??= GetComponentInParent<UIMenuWindowHandler>();
        }

        protected virtual void Interact(){}
    }
}
