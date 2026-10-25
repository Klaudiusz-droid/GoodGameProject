using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;

    [SerializeField] private int WalkSpeed = 5;
    [SerializeField] private int RunSpeed = 9;
    [SerializeField] private int Gravity = -20;

    [SerializeField] private Transform playerCamera;
    [SerializeField] private float mouseSensitivity = 0.1f;
    [SerializeField] private int UpCameraLookBlock = 90;
    [SerializeField] private int DownCameraLookBlock = -90;

    private Vector2 moveInput;
    private Vector2 lookInput;

    private float cameraRotationX;
    private float verticalVelocity;

    private bool isRunning;



    private void Start()
    {
        controller = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        PlayerMoving();
        CameraLook();
        ApplyGravity();
    }




	//Daje PlayerMoving kierunek ruchu
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
	
	//Daje CameraLook kierunek w którym ma się patrzyć
    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

	//Zaczyna i kończy bieg xd
    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.performed)
            isRunning = true;
        else if (context.canceled)
            isRunning = false;
    }





	//Wprowadza postać w ruch
    private void PlayerMoving()
    {
        Vector3 direction = transform.right * moveInput.x + transform.forward * moveInput.y;

        direction = Vector3.ClampMagnitude(direction, 1f);

        float currentSpeed = isRunning ? RunSpeed : WalkSpeed;
        Vector3 horizontalVelocity = direction * currentSpeed;

        controller.Move(horizontalVelocity * Time.deltaTime);
    }

	//Ruch kamery
    private void CameraLook()
    {
        cameraRotationX -= lookInput.y * mouseSensitivity;
        cameraRotationX = Mathf.Clamp(cameraRotationX, DownCameraLookBlock, UpCameraLookBlock);

        if (playerCamera != null)
        {
            playerCamera.localRotation = Quaternion.Euler(cameraRotationX, 0f, 0f);
        }

        transform.Rotate(0f, lookInput.x * mouseSensitivity, 0f);
    }
	
	// Grawitacja
    private void ApplyGravity()
    {
        if (controller.isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = -2f;
        }

        verticalVelocity += Gravity * Time.deltaTime;

        controller.Move(Vector3.up * verticalVelocity * Time.deltaTime);
    }
}