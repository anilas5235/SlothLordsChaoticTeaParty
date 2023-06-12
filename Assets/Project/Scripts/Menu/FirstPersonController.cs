using System;
using System.Collections;
using Project.Scripts.General;
using UnityEngine;

namespace Project.Scripts.Menu
{
    [RequireComponent(typeof(CharacterController))]
    public class FirstPersonController : MonoBehaviour
    {
        public bool CanMove { get; private set; } = true;
        private bool IsSprinting => canSprint && Input.GetButton(sprintInput);
        private bool ShouldJump => Input.GetButtonDown(jumpInput) && myCharacterController.isGrounded;

        private bool ShouldCrouch => canCrouch && Input.GetButton(crouchInput);
        private bool isCrouching = false;

        [Header("Options")]
        [SerializeField] private bool canSprint = true;
        [SerializeField] private bool canJump = true;
        [SerializeField] private bool canCrouch = true;

        [Header("Controls")]
        [SerializeField] private string sprintInput = "Sprint";
        [SerializeField] private string jumpInput = "Jump";
        [SerializeField] private string crouchInput = "Crouch";

        [Header("Movement Parameters")]
        [SerializeField] private float walkSpeed = 3f;
        [SerializeField] private float sprintSpeed = 6f;

        [Header("Look Parameters")]
        [SerializeField, Range(1, 10)] private float lookSpeedX = 2f;
        [SerializeField, Range(1, 10)] private float lookSpeedY = 2f;
        [SerializeField, Range(1, 180)] private float upperLookLimit= 80f;
        [SerializeField, Range(1, 180)] private float lowerLookLimit= 80f;

        [Header("Jump Parameters")] 
        [SerializeField] private float jumpForce = 8f;
        [SerializeField] private float gravity = 30f;

        [Header("Crouch Parameters")] 
        [SerializeField] private float transitionTime = .5f;
        [SerializeField] private float standingHeight = 2f;
        [SerializeField] private Vector3 standingBodyCenter = new Vector3(0,-.6f,0);
        [SerializeField] private float crouchingHeight = 1f;
        [SerializeField] private Vector3 crouchingBodyCenter = new Vector3(0,-.1f,0);
        

        public Camera PlayerCamera { get; private set; }
        private CharacterController myCharacterController;

        private Vector3 moveDirection;
        private Vector2 currentInput;
        private Coroutine transition;

        private float rotationX = 0;

        private void Awake()
        {
            PlayerCamera = GetComponentInChildren<Camera>();
            myCharacterController = GetComponent<CharacterController>();
            CanMove = true;
        }

        private void Start()
        { 
            CursorManager.Instance.DeActivateCursor();
        }

        void Update()
        {
            if (CanMove)
            {
                HandelMovementInput();
                HandleMouseLook();
                if(canCrouch) HandelCrouch();
                if(canJump) HandelJump();
                ApplyFinalMovement();
            }
        }

        private void HandelMovementInput()
        {
            float speed = IsSprinting ? sprintSpeed : walkSpeed;
            speed *= isCrouching ? .5f : 1;
            currentInput = new Vector2(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal")) * speed;

            float moveDirectionY = moveDirection.y;

            moveDirection = (transform.TransformDirection(Vector3.forward) * currentInput.x) +
                            (transform.TransformDirection(Vector3.right) * currentInput.y);
            moveDirection.y = moveDirectionY;
        }

        private void HandelJump()
        {
            if (ShouldJump)
            {
                float jumpMulti = isCrouching ? .5f : 1;
                moveDirection.y = jumpForce * jumpMulti;
            }
        }

        private void HandelCrouch()
        {
            if (isCrouching)
            {
                if (!ShouldCrouch)
                {
                    if (transition != null) StopCoroutine(transition);
                    transition = StartCoroutine(TransitionCrouch(standingBodyCenter, standingHeight));
                    isCrouching = false;
                }
            }
            else
            {
                if (ShouldCrouch)
                {
                    if (transition != null) StopCoroutine(transition);
                    transition = StartCoroutine(TransitionCrouch(crouchingBodyCenter, crouchingHeight));
                    isCrouching = true;
                }
            }
        }

        private void HandleMouseLook()
        {
            rotationX -= Input.GetAxis("Mouse Y") * lookSpeedY;
            rotationX = Mathf.Clamp(rotationX, -upperLookLimit, lowerLookLimit);
            PlayerCamera.transform.localRotation = Quaternion.Euler(rotationX,0,0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X")*lookSpeedX,0);
        }

        private void ApplyFinalMovement()
        {
            if (!myCharacterController.isGrounded) moveDirection.y -= gravity * Time.deltaTime;
            myCharacterController.Move(moveDirection * Time.deltaTime);
        }

        public void FreezePlayerToggle() => CanMove = !CanMove;

        private IEnumerator TransitionCrouch(Vector3 bodyCenter, float height)
        {
            float time = 0;
            Vector3 originalBodyCenter = myCharacterController.center;
            float originalHeight = myCharacterController.height;

            while (time < transitionTime)
            {
                yield return new WaitForFixedUpdate();
                float t = time / transitionTime;
                myCharacterController.center = Vector3.Lerp(originalBodyCenter, bodyCenter,t);
                myCharacterController.height = Mathf.Lerp(originalHeight, height, t);
                time += Time.fixedDeltaTime;
            }

            myCharacterController.center = bodyCenter;
            myCharacterController.height = height;
            transition = null;
        }
    }
}
