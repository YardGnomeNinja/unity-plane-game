using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using ExtensionMethods;

public class Player : MonoBehaviour
{
    // Useful when not using a Rigidbody
    CharacterController characterController;
    NetworkedObject networkedObject;
    public Camera playerCamera;
    public float speed = 5f;
    public float jumpVelocity = 5f;
    private float gravity = -9.81f;
    Vector3 playerVelocity;
	public float mouseSensitivity = 5.0f;
	float verticalRotation = 0;
	public float upDownRange = 80.0f;

    // Start is called before the first frame update
    void Start()
    {
        // In the event any gameObject in the Player's hierarchy was disabled, reenable them
        //gameObject.SetActiveRecursivelyExt(true);

        characterController = GetComponent<CharacterController>();
        networkedObject = GetComponent<NetworkedObject>();
        //playerCamera = (Camera)GameObject.FindObjectOfType<Camera>();
        
        foreach (Camera camera in Resources.FindObjectsOfTypeAll(typeof(Camera)))
        {
            if (camera.transform.parent == networkedObject.transform)
            {
                playerCamera = camera;
                playerCamera.gameObject.SetActive(true);
            }
            else 
            {
                camera.gameObject.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    // Note: For resons I don't yet comprehend, moving the gravity calculations below movement causes undesireable effects for isGrounded. Hence they happen first.
    void Update()
    {
        // Calculate Gravity
        if(characterController.isGrounded)
        {
            // If the character is on the ground, continue to apply downward force to keep them there
            playerVelocity.y = -1;

            // Only allow the character to jump if they are on the ground
            if(Input.GetKeyDown(KeyCode.Space)) {
                // If the player presses space, overcome gravity to jump
                playerVelocity.y = jumpVelocity;
            }
        }
        else
        {
            // If the character is not on the ground, increase the force of gravity gradually
            playerVelocity.y += gravity * Time.deltaTime;
        }
        
        // Mouse input
		float rotLeftRight = Input.GetAxis("Mouse X") * mouseSensitivity;
		transform.Rotate(0, rotLeftRight, 0);

		verticalRotation -= Input.GetAxis("Mouse Y") * mouseSensitivity;
		verticalRotation = Mathf.Clamp(verticalRotation, -upDownRange, upDownRange);
		playerCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);

        // Movement input
        var strafeInput = Input.GetAxis("Horizontal");
        var forwardInput = Input.GetAxis("Vertical");

        playerVelocity.x = strafeInput * speed;
        playerVelocity.z = forwardInput * speed;

        // Make it so
		Vector3 movement = new Vector3(playerVelocity.x, playerVelocity.y, playerVelocity.z);
		
		movement = transform.rotation * movement;
		
		characterController.Move(movement * Time.deltaTime);
    }
}
