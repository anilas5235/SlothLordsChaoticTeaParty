using System;
using UnityEngine;

namespace Project.Scripts.Menu
{
    public class PlayerInteractionManager : MonoBehaviour
    {
        private Transform camTransform;

        [SerializeField] private float maxReach = 50f;
        private IInteractable targetGameObject;

        private void Awake()
        {
            camTransform = GetComponentInChildren<Camera>().transform;
        }

        private void Update()
        {
            Ray ray = new Ray(camTransform.position, camTransform.TransformDirection(Vector3.forward));
            RaycastHit[] hits = Physics.RaycastAll(ray, maxReach);

            bool noInteractables = true;


            foreach (RaycastHit hit in hits)
            {
                if (hit.transform.TryGetComponent(typeof(IInteractable), out Component component))
                {
                    IInteractable newTarget = ((IInteractable)component);
                    if (targetGameObject == null)
                    {
                        targetGameObject = newTarget;
                        targetGameObject.Highlight();
                    }
                    else if(targetGameObject != newTarget)
                    {
                         targetGameObject.Highlight();
                         targetGameObject = newTarget;
                         targetGameObject.Highlight();
                    }
                    noInteractables = false;
                    break;
                }
            }

            if (noInteractables && targetGameObject != null)
            {
                targetGameObject.Highlight();
                targetGameObject = null;
            }

            if (Input.GetMouseButtonDown(0))
            {
                if(targetGameObject != null) targetGameObject.Interact();
            }


            Debug.DrawRay(camTransform.position, camTransform.TransformDirection(Vector3.forward), Color.red);
        }
    }
}
