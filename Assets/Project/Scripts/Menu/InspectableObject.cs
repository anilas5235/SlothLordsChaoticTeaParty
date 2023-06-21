using Project.Scripts.UIScripts.Windows;
using UnityEngine;

namespace Project.Scripts.Menu
{
    public class InspectableObject : InteractableObject
    {
        [SerializeField] private float inspectingDistance = 1f;
        private Transform cameraTransform;
        private bool inspecting;
        private Vector3 originalPosition, originalRotation;
        private Vector3 addRotation;

        private FirstPersonController controller;

        private const float RotationSpeed = 1000;

        protected override void Awake()
        {
            base.Awake();
            originalPosition = transform.position;
            originalRotation = transform.rotation.eulerAngles;
            controller = FindObjectOfType<FirstPersonController>();
        }

        protected virtual void Start()
        {
            cameraTransform = controller.PlayerCamera.transform;
        }

        public override void Interact()
        {
            base.Interact();
            ToggleInspection();
        }

        private void ToggleInspection()
        {
            if(MenuWindowsMaster.Instance.MenuActive) return;
            inspecting = !inspecting;
            if (inspecting)
            {
                transform.position = cameraTransform.position + cameraTransform.TransformDirection(Vector3.forward) * inspectingDistance;
                transform.forward = (cameraTransform.position - transform.position).normalized;
                addRotation = transform.localEulerAngles;
                transform.rotation = Quaternion.Euler(0,0,0);
                controller.FreezePlayer();
                myOutline.eraseRenderer = true;
                transform.SetParent(null);
            }
            else
            {
                transform.position = originalPosition;
                transform.forward = Vector3.forward;
                transform.rotation = Quaternion.Euler( originalRotation);
                controller.UnFreezePlayer();
                myOutline.eraseRenderer = false;
            }
        }

        private void Update()
        {
            if (inspecting)
            {
                Vector2 mouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) * (RotationSpeed * Time.deltaTime);
                
                addRotation.x -= mouseInput.y;
                if (addRotation.x >= 360 || addRotation.x <= -360) addRotation.x = 0;
                addRotation.y -= mouseInput.x;
                if (addRotation.y >= 360 || addRotation.y <= -360) addRotation.y = 0;

                transform.localEulerAngles =  addRotation;

                if (Input.GetButtonDown("Cancel") || Input.GetMouseButtonDown(1))
                {
                    ToggleInspection();
                }
            }
        }
    }
}
