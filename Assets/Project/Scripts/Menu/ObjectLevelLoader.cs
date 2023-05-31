using System;
using UnityEngine;

namespace Project.Scripts.Menu
{
    public class ObjectLevelLoader : MonoBehaviour, IInteractable

    {
        [SerializeField] private MenuManager.Scenes sceneToLoad;
        private Vector3 scale;
        private bool hightlighted = false;

        private void Awake()
        {
            scale = transform.localScale;
        }

        public void Interact()
        {
            MenuManager.instance.LoadScene(sceneToLoad);
        }

        public void Highlight()
        {
            hightlighted = true;
        }

        private void Update()
        {
            if (hightlighted) transform.localScale = scale + new Vector3(.2f, .2f, .2f);
            else transform.localScale = scale;

            hightlighted = false;
        }
    }
}
