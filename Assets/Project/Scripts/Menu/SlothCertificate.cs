using Project.Scripts.General;
using UnityEngine;

namespace Project.Scripts.Menu
{
    public class SlothCertificate : InspectableObject
    {
        protected override void Start()
        {
            base.Start();
            gameObject.SetActive(SaveSystem.Instance.GetActiveSave().unlockedEndings[1]);
        }
    }
}
