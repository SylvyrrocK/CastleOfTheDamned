using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
        public Transform cameraTr;
        public InputActionAsset inputAsset;
        public CharacterController charController;
        public bool canLookAround = true;
        public bool isFreeCursor = false;
        public float cameraSensitivityVertical;
        public float cameraSensitivityHorizontal;
        public bool canMove = true;
        public float moveSpeed;
        Vector2 moveInput = new Vector2(0f, 0f);
        float verticalFraction = 0.5f;
        Vector2 mouseInput = new Vector2(0f, 0f);
        public float gravity = -9.81f;
        public bool canJump = true;
        public float jumpHeight = 100f;
        public float maxFallSpeed = -500f;
        float verticalSpeed = 0f;
        public Transform groundCheckTr;
        bool isGrounded;
        public LayerMask groundLayer;
        public float maxStamina = 100f;
        public float sprintSpeed;
        public float currentStamina;
        public bool isSprinting;
        bool tempDisableSprint = false;
        public bool canUseItems = true;
        public UnityEvent<Vector2> onMousePosition;
        public UnityEvent<bool, bool> onPrimary;
        public UnityEvent<bool, bool> onSecondary;
        public UnityEvent<float> onMouseWheel;
        public UnityEvent onEscape;
        public UnityEvent onJump;
        void Update()
        {
                RotateCamera();
                PlayerGravity();
                ChangeStamina();
                MovePlayer();
        }

        void RotateCamera()
        {
                if (!canLookAround)
                {
                        return;
                }

                verticalFraction = Mathf.Clamp(verticalFraction + mouseInput.y * cameraSensitivityVertical * Time.deltaTime, 0f, 1f);

                cameraTr.localRotation = Quaternion.Euler(90f - verticalFraction * 180f, cameraTr.rotation.y, cameraTr.rotation.z);

                transform.Rotate(Vector3.up, mouseInput.x * cameraSensitivityHorizontal * Time.deltaTime);
        }

        public void ForceRotateCamera(Vector2 vector)
        {
                verticalFraction += vector.y / 180f;
                transform.Rotate(Vector3.up, vector.x);
        }

        void ChangeStamina()
        {
                if (currentStamina <= 0f)
                {
                        isSprinting = false;
                        tempDisableSprint = true;
                }
                else if (currentStamina >= maxStamina)
                {
                        tempDisableSprint = false;
                }

                if (isSprinting && !tempDisableSprint)
                {
                        currentStamina = Mathf.Clamp(currentStamina - 1f, 0f, maxStamina);
                }
                else
                {
                        currentStamina = Mathf.Clamp(currentStamina + 2f, 0f, maxStamina);
                }
        }

        void PlayerGravity()
        {
                if (!isGrounded)
                {
                        if (verticalSpeed > maxFallSpeed)
                        {
                                verticalSpeed += gravity;
                        }
                }
                else
                {
                        if (verticalSpeed < 0f)
                        {
                                verticalSpeed = 0f;
                        }
                }

                charController.Move(new Vector3(0f, verticalSpeed * Time.deltaTime, 0f));
        }

        void MovePlayer()
        {
                if (!canMove)
                {
                        return;
                }

                float actualSpeed;
                if (isSprinting && !tempDisableSprint)
                {
                        actualSpeed = sprintSpeed;
                }
                else
                {
                        actualSpeed = moveSpeed;
                }

                charController.Move(transform.rotation * new Vector3(moveInput.x * actualSpeed, 0f, moveInput.y * actualSpeed) * Time.deltaTime);
        }

        public void FreeCursor(bool isCursorFree)
        {
                if (isCursorFree)
                {
                        Cursor.lockState = CursorLockMode.None;
                        canLookAround = false;
                        isFreeCursor = true;
                }
                else
                {
                        Cursor.lockState = CursorLockMode.Locked;
                        canLookAround = true;
                        isFreeCursor = false;
                }
        }

        void FixedUpdate()
        {
                isGrounded = Physics.SphereCast(groundCheckTr.position, 0.1f, -Vector3.up, out RaycastHit _, 0.25f, groundLayer);
        }

        void OnDrawGizmos()
        {
                Gizmos.DrawSphere(groundCheckTr.position, 0.1f);
                Gizmos.DrawSphere(groundCheckTr.position - Vector3.up * 0.25f, 0.1f);
        }

        void OnEnable()
        {
                currentStamina = maxStamina;

                Cursor.lockState = CursorLockMode.Locked;

                inputAsset.Enable();

                var actionMap = inputAsset.FindActionMap("Character");

                var onMouse = inputAsset.FindAction("Mouse");
                var onMousePosition = inputAsset.FindAction("MousePosition");
                var onMouseWheel = inputAsset.FindAction("MouseWheel");
                var onMove = inputAsset.FindAction("Move");
                var onJump = inputAsset.FindAction("Jump");
                var onSprint = inputAsset.FindAction("Sprint");
                var onFreeCursor = inputAsset.FindAction("FreeCursor");
                var onPrimary = inputAsset.FindAction("Primary");
                var onSecondary = inputAsset.FindAction("Secondary");
                var onEscape = inputAsset.FindAction("Esc");

                onMouse.performed += OnMouse;
                onMouse.canceled += OnMouseCancelled;
                onMousePosition.performed += OnMousePosition;
                onMouseWheel.performed += OnMouseWheel;
                onMove.performed += OnMove;
                onMove.canceled += OnMoveCancelled;
                onJump.performed += OnJump;
                onSprint.performed += OnSprint;
                onSprint.canceled += OnSprintCancelled;
                onFreeCursor.performed += OnFreeCursor;
                onFreeCursor.canceled += OnFreeCursorCanceled;
                onPrimary.performed += OnPrimary;
                onPrimary.canceled += OnPrimaryCanceled;
                onSecondary.performed += OnSecondary;
                onSecondary.canceled += OnSecondaryCanceled;
                onEscape.performed += OnEscape;
        }

        void OnDisable()
        {
                Cursor.lockState = CursorLockMode.None;

                inputAsset.Disable();
        }

        void OnMouse(InputAction.CallbackContext context)
        {
                mouseInput = context.ReadValue<Vector2>();
        }

        void OnMouseCancelled(InputAction.CallbackContext context)
        {
                mouseInput = Vector2.zero;
        }

        void OnMousePosition(InputAction.CallbackContext context)
        {
                onMousePosition.Invoke(context.ReadValue<Vector2>());
        }

        void OnMouseWheel(InputAction.CallbackContext contex)
        {
                onMouseWheel.Invoke(contex.ReadValue<float>());
        }

        void OnMove(InputAction.CallbackContext context)
        {
                moveInput = context.ReadValue<Vector2>();
        }

        void OnMoveCancelled(InputAction.CallbackContext context)
        {
                moveInput = Vector2.zero;
        }

        void OnJump(InputAction.CallbackContext context)
        {
                onJump.Invoke();
                if (isGrounded && canJump)
                {
                        verticalSpeed = jumpHeight;
                }
        }

        void OnSprint(InputAction.CallbackContext context)
        {
                isSprinting = true;
        }

        void OnSprintCancelled(InputAction.CallbackContext context)
        {
                isSprinting = false;
        }

        void OnFreeCursor(InputAction.CallbackContext context)
        {
                FreeCursor(true);
        }

        void OnFreeCursorCanceled(InputAction.CallbackContext context)
        {
                FreeCursor(false);
        }

        void OnPrimary(InputAction.CallbackContext context)
        {
                if (canUseItems)
                {
                        onPrimary.Invoke(true, isFreeCursor);
                }
        }

        void OnPrimaryCanceled(InputAction.CallbackContext context)
        {
                if (canUseItems)
                {
                        onPrimary.Invoke(false, isFreeCursor);
                }
        }

        void OnSecondary(InputAction.CallbackContext context)
        {
                if (canUseItems)
                {
                        onSecondary.Invoke(true, isFreeCursor);
                }
        }

        void OnSecondaryCanceled(InputAction.CallbackContext context)
        {
                if (canUseItems)
                {
                        onSecondary.Invoke(false, isFreeCursor);
                }
        }

        void OnEscape(InputAction.CallbackContext context)
        {
                onEscape.Invoke();
        }
}
