using UnityEngine;

namespace ModularCheatCodeSystem
{
    /// <summary>
    /// A simple and clean First-Person Controller.
    /// This script handles player movement (WASD), jumping, gravity, and mouse-look.
    /// It's designed to be a lightweight, easy-to-understand starting point for a demo scene.
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class SimpleFPSController : MonoBehaviour
    {
        [Header("Camera Settings")]
        [Tooltip("The transform of the camera attached to the player.")]
        public Transform playerCamera;
        [Tooltip("Mouse sensitivity for looking around.")]
        public float lookSpeed = 2.0f;
        [Tooltip("The vertical look limit in degrees to prevent the camera from flipping over.")]
        public float lookXLimit = 80.0f;

        [Header("Movement Settings")]
        [Tooltip("The walking speed of the player.")]
        public float moveSpeed = 7.0f;
        [Tooltip("The force of gravity applied to the player.")]
        public float gravity = -19.62f;
        [Tooltip("The height the player can jump.")]
        public float jumpHeight = 1.5f;

        // --- Private Internal Variables ---
        private CharacterController _characterController;
        private float _rotationX = 0;
        private Vector3 _playerVelocity;
        private bool _isCursorLocked = true;

        #region Unity Lifecycle Methods
        private void Start()
        {
            _characterController = GetComponent<CharacterController>();
            UpdateCursorState();
        }

        private void Update()
        {
            HandleCursorLocking();

            // Only allow movement and looking around if the cursor is locked.
            if (_isCursorLocked)
            {
                HandleMovementAndGravity();
                HandleCameraLook();
            }
        }
        #endregion

        #region Core Logic
        /// <summary>
        /// Handles player movement based on keyboard input and applies gravity.
        /// </summary>
        private void HandleMovementAndGravity()
        {
            bool isGrounded = _characterController.isGrounded;

            // Reset vertical velocity when grounded
            if (isGrounded && _playerVelocity.y < 0)
            {
                _playerVelocity.y = -2f;
            }

            // --- Movement Input (WASD / Arrow Keys / Controller) ---
            float moveX = Input.GetAxis("Horizontal");
            float moveZ = Input.GetAxis("Vertical");

            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 right = transform.TransformDirection(Vector3.right);
            Vector3 moveDirection = (forward * moveZ + right * moveX).normalized;

            // Apply movement speed
            _characterController.Move(moveDirection * moveSpeed * Time.deltaTime);

            // --- Jumping Logic ---
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                // The formula for jumping to a specific height: sqrt(height * -2 * gravity)
                _playerVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }

            // Apply gravity
            _playerVelocity.y += gravity * Time.deltaTime;

            // Apply final vertical movement
            _characterController.Move(_playerVelocity * Time.deltaTime);
        }

        /// <summary>
        /// Handles camera rotation based on mouse input.
        /// </summary>
        private void HandleCameraLook()
        {
            // --- Vertical Look (Up/Down) ---
            _rotationX -= Input.GetAxis("Mouse Y") * lookSpeed;
            _rotationX = Mathf.Clamp(_rotationX, -lookXLimit, lookXLimit);
            playerCamera.localRotation = Quaternion.Euler(_rotationX, 0, 0);

            // --- Horizontal Look (Left/Right) ---
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }
        #endregion

        #region Cursor Management
        /// <summary>
        /// Toggles the cursor lock state when the Escape key is pressed or locks it on mouse click.
        /// </summary>
        private void HandleCursorLocking()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _isCursorLocked = !_isCursorLocked;
                UpdateCursorState();
            }
            else if (Input.GetMouseButtonDown(0) && !_isCursorLocked)
            {
                _isCursorLocked = true;
                UpdateCursorState();
            }
        }

        /// <summary>
        /// Updates the cursor's visibility and lock state based on the _isCursorLocked variable.
        /// </summary>
        private void UpdateCursorState()
        {
            if (_isCursorLocked)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
        #endregion
    }
}
