using UnityEngine;

namespace Project.Scripts.Menu
{
    public class ObjectLevelLoader : InteractableObject

    {
        [SerializeField] private int levelID;

        public override void Interact()
        {
            SceneMaster.Instance.ChangeToLevel(levelID);
        }
    }
}
