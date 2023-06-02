using UnityEngine;

namespace Project.Scripts.Menu
{
    public class ObjectLevelLoader : InteractableObject

    {
        [SerializeField] private MenuManager.Scenes sceneToLoad;

        public override void Interact()
        {
            MenuManager.instance.LoadScene(sceneToLoad);
        }
    }
}
