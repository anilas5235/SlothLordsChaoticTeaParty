using UnityEngine;

namespace Project.Scripts.Menu
{
    public class InspectableObject : InteractableObject
    {
        [SerializeField] private float InspectingDistance = 1f;
        private Transform _cameraTransform;
        private bool Inspecting;
        private Vector3 originalPosition, originalRotation;
        private Vector3 addRotation;

        private FirstPersonController Controller;

        private const float rotationSpeed = 1000;

        protected override void Awake()
        {
            base.Awake();
            originalPosition = transform.position;
            originalRotation = transform.rotation.eulerAngles;
            Controller = FindObjectOfType<FirstPersonController>();
        }

        private void Start()
        {
            _cameraTransform = Controller.PlayerCamera.transform;
        }

        public override void Interact()
        {
            ToggleInspection();
        }

        private void ToggleInspection()
        {
            Inspecting = !Inspecting;
            if (Inspecting)
            {
                transform.position = _cameraTransform.position + _cameraTransform.TransformDirection(Vector3.forward) * InspectingDistance;
                transform.forward = (_cameraTransform.position - transform.position).normalized;
                addRotation = transform.localEulerAngles;
                Controller.FreezePlayerToggle();
                myOutline.eraseRenderer = true;
                transform.SetParent(null);
            }
            else
            {
                transform.position = originalPosition;
                transform.forward = Vector3.forward;
                transform.rotation = Quaternion.Euler( originalRotation);
                Controller.FreezePlayerToggle();
                myOutline.eraseRenderer = false;
            }
        }

        private void Update()
        {
            if (Inspecting)
            {
                Vector2 mouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) * (rotationSpeed * Time.deltaTime);
                
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
