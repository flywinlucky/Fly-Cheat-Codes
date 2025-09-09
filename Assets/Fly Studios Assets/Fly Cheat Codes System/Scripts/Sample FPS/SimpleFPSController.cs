using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class SimpleFPSController : MonoBehaviour
{
    [Header("Camera Settings")]
    public Transform playerCamera;
    [Tooltip("Sensibilitatea mouse-ului pentru rotație")]
    public float lookSpeed = 2.0f;
    [Tooltip("Limita de rotație verticală a camerei (în grade)")]
    public float lookXLimit = 80.0f;

    [Header("Movement Settings")]
    [Tooltip("Viteza de mișcare a jucătorului")]
    public float moveSpeed = 7.0f;
    [Tooltip("Forța gravitației aplicate jucătorului")]
    public float gravity = -19.62f;
    [Tooltip("Înălțimea săriturii")]
    public float jumpHeight = 1.5f;

    // Variabile interne private
    private CharacterController characterController;
    private float rotationX = 0;
    private float verticalVelocity = 0;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleMovementAndGravity();
        HandleCameraLook();
    }

    private void HandleMovementAndGravity()
    {
        bool isGrounded = characterController.isGrounded;

        if (isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -2f;
        }

        // --- Colectarea input-ului de mișcare (W/A/S/D) ---

        // Mișcare înainte (W) / înapoi (S)
        float verticalInput = 0f;
        if (Input.GetKey(KeyCode.W)) // *** CORECTURA ESTE AICI ***
        {
            verticalInput = 1f;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            verticalInput = -1f;
        }

        // Mișcare stânga (A) / dreapta (D)
        float horizontalInput = 0f;
        if (Input.GetKey(KeyCode.D))
        {
            horizontalInput = 1f;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            horizontalInput = -1f;
        }

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        Vector3 movement = (forward * verticalInput + right * horizontalInput).normalized * moveSpeed;

        // --- Logica săriturii ---
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Aplicăm gravitația
        verticalVelocity += gravity * Time.deltaTime;

        // Aplicăm mișcarea finală
        characterController.Move(movement * Time.deltaTime + new Vector3(0, verticalVelocity, 0) * Time.deltaTime);
    }

    private void HandleCameraLook()
    {
        rotationX -= Input.GetAxis("Mouse Y") * lookSpeed;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        playerCamera.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
    }
}