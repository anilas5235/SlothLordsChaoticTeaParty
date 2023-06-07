using UnityEngine;

namespace Project.Scripts.UIScripts.InteractableUI
{
    public abstract class CustomUIInteractableBase<T> : MonoBehaviour
    {
        protected T myInteractable;
        protected UIWindowHandler myWindowHandler;

        protected virtual void OnEnable()
        {
            myInteractable ??= GetComponent<T>();
            myWindowHandler ??= GetComponentInParent<UIWindowHandler>();
        }

        protected virtual void Interact(){}
    }
}
