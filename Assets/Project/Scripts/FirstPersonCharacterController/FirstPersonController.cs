using UnityEngine;

namespace Project.Scripts.FirstPersonCharacterController
{
    [RequireComponent(typeof(CharacterController))]
    public class FirstPersonController : MonoBehaviour
    {
        public bool CanMove { get; private set; } = true;

        [Header("Movement Parameters")]
        [SerializeField] private float walkSpeed = 3f;
        [SerializeField] private float gravity = 30f;

        [Header("Look Parameters")]
        [SerializeField, Range(1, 10)] private float lookSpeedX = 2f;
        [SerializeField, Range(1, 10)] private float lookSpeedY = 2f;
        [SerializeField, Range(1, 180)] private float upperLookLimit= 80f;
        [SerializeField, Range(1, 180)] private float lowerLookLimit= 80f;

        private Camera playerCamera;
        private CharacterController myCharacterController;

        private Vector3 moveDirection;
        private Vector2 currentInput;

        private float rotaionX = 0;

        private void Awake()
        {
            playerCamera = GetComponentInChildren<Camera>();
            myCharacterController = GetComponent<CharacterController>();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        void Update()
        {
            if (CanMove)
            {
                HandelMovementInput();
                HandleMouseLook();
                ApplyFinalMovement();
            }
        }

        private void HandelMovementInput()
        {
            currentInput = new Vector2(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal")) * walkSpeed;

            float moveDirectionY = moveDirection.y;

            moveDirection = (transform.TransformDirection(Vector3.forward) * currentInput.x) +
                            (transform.TransformDirection(Vector3.right) * currentInput.y);
            moveDirection.y = moveDirectionY;
        }

        private void HandleMouseLook()
        {
            rotaionX -= Input.GetAxis("Mouse Y") * lookSpeedY;
            rotaionX = Mathf.Clamp(rotaionX, -upperLookLimit, lowerLookLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotaionX,0,0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X")*lookSpeedX,0);
        }

        private void ApplyFinalMovement()
        {
            if (!myCharacterController.isGrounded) moveDirection.y -= gravity * Time.deltaTime;
            myCharacterController.Move(moveDirection * Time.deltaTime);
        }
    }
}
