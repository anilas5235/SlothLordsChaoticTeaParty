using Project.Scripts.General;
using UnityEngine;

namespace Project.Scripts.Menu
{
    public class SlothCertificate : InteractableObject
    {
        protected  void Start()
        {
            gameObject.SetActive(SaveSystem.Instance.GetActiveSave().unlockedEndings[1]);
        }

        public override void Interact()
        {
            base.Interact();
            SceneMaster.Instance.ChangeToEndingDialog(21);
        }
    }
}
