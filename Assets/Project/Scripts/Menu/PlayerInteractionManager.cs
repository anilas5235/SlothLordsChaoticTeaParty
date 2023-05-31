using System;
using UnityEngine;

namespace Project.Scripts.Menu
{
    public class PlayerInteractionManager : MonoBehaviour
    {
        private Transform camTranform;

        [SerializeField] private float maxReach = 50f;

        private void Awake()
        {
            camTranform = GetComponentInChildren<Camera>().transform;
        }

        private void Update()
        {
            Ray ray = new Ray(camTranform.position, camTranform.TransformDirection(Vector3.forward));
                         RaycastHit[] hits = Physics.RaycastAll(ray, maxReach);
            if (Input.GetMouseButtonDown(0))
            {
                foreach (RaycastHit hit in hits)
                {
                    if (hit.transform.TryGetComponent(typeof(IInteractable), out Component component))
                    {
                        ((IInteractable)component).Interact();
                    }
                }
            }
            
            foreach (RaycastHit hit in hits)
            {
                if (hit.transform.TryGetComponent(typeof(IInteractable), out Component component))
                {
                    ((IInteractable)component).Highlight();
                }
            }
            
            Debug.DrawRay(camTranform.position, camTranform.TransformDirection(Vector3.forward), Color.red);
        }
    }
}
