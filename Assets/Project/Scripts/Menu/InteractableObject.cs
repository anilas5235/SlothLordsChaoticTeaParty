using System;
using cakeslice;
using Project.Scripts.General;
using UnityEngine;

namespace Project.Scripts.Menu
{
    [RequireComponent(typeof(Outline))]
    public abstract class InteractableObject : MonoBehaviour, IInteractable,IShouldForceAwake
    {
        protected Outline myOutline;
        public bool hightlighted { get; protected set; } = false;

        protected virtual void Awake()
        {
            myOutline = GetComponent<Outline>();
            myOutline.eraseRenderer = true;
            myOutline.color = 2;
        }

        public abstract void Interact();

        public virtual void Highlight()
        {
           ToggleHighLight();
        }

        protected void ToggleHighLight()
        {
            hightlighted = !hightlighted;
            myOutline.eraseRenderer = !hightlighted;
        }

        public void ForceAwake()
        {
            Awake();
        }
    }
}
