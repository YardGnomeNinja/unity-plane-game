using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using ExtensionMethods;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    #region Base Objects
        private CharacterController characterController;
        private NetworkedObject networkedObject;
        public Camera playerCamera;
    #endregion

    #region Input
        public float mouseSensitivity = 5.0f;
    #endregion

    #region Movement
        private float gravity = -9.81f;
        public float jumpVelocity = 5f;
        private Vector3 playerVelocity;
        public float speed = 5f;
        public float upDownRange = 80.0f;
        private float verticalRotation = 0f;
    #endregion
        
    #region Stats
        public Image exhaustionBar;
        public float exhaustionCurrent = 0f;
        public float exhaustionRate = 0.1f;

        public Image healthBar;
        public float healthCurrent = 1f;

        public Image hungerBar;
        public float hungerCurrent = 0f;

        public Image insanityBar;
        public float insanityCurrent = 0f;
        public float insanityRate = 0.05f;

        public Image intoxicationBar;
        public float intoxicationCurrent = 0f;

        public float maxStat = 1f;

        public float metabolism = 0.5f;                                             // The base rate at which the player gains Hunger, Thirst, Intoxication, etc.
        float metabolicRefreshRate { get { return statRefreshRate * metabolism; }}  // The time between updates to metabolic functions
        float metabolicRefreshCountdownTimer = 0f;

        private float statRefreshCountdownTimer = 0f;
        public float statRefreshRate = 1f;                                         // The time between updates to non-metabolic functions

        public Image thirstBar;
        public float thirstCurrent = 0f;
    #endregion

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

    private void FixedUpdate() {
        UpdateCurrentStats();
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

    private void UpdateCurrentStats()
    {
        // Debug.Log(statRefreshRate);
        // Debug.Log(metabolicRefreshRate);
        statRefreshCountdownTimer -= Time.deltaTime;
        metabolicRefreshCountdownTimer -= Time.deltaTime;
        
        if (statRefreshCountdownTimer <= 0)
        {
            insanityCurrent = Mathf.Clamp(insanityCurrent + insanityRate, 0, maxStat);
            exhaustionCurrent = Mathf.Clamp(exhaustionCurrent + exhaustionRate, 0, maxStat);
            statRefreshCountdownTimer = statRefreshRate;    // Restart the timer for updating stats
        }

        if (metabolicRefreshCountdownTimer <= 0)
        {
            hungerCurrent += 0.01f;
            hungerCurrent = Mathf.Clamp(hungerCurrent, 0, maxStat);
            thirstCurrent = Mathf.Clamp(thirstCurrent++, 0, maxStat);
            intoxicationCurrent = Mathf.Clamp(intoxicationCurrent--, 0, maxStat);

            metabolicRefreshCountdownTimer = metabolicRefreshRate;  // Restart the timer for updating metabolic functions
        }

        // Update bars
        UpdateExhaustionBar();
        UpdateHealthBar();
        UpdateHungerBar();
        UpdateInsanityBar();
        UpdateIntoxicationBar();
        UpdateThirstBar();
    }

    private void UpdateExhaustionBar()
    {
        exhaustionBar.fillAmount = exhaustionCurrent;

        // Green to Red
        exhaustionBar.color = new Color(exhaustionBar.fillAmount, 1 - exhaustionBar.fillAmount, 0);
    }

    private void UpdateHealthBar()
    {
        healthBar.fillAmount = healthCurrent;

        // Red to Green
        healthBar.color = new Color(1 - healthBar.fillAmount, healthBar.fillAmount, 0);
    }

    private void UpdateHungerBar()
    {
        hungerBar.fillAmount = hungerCurrent;

        // Green to Red
        hungerBar.color = new Color(hungerBar.fillAmount, 1 - hungerBar.fillAmount, 0);
    }

    private void UpdateInsanityBar()
    {
        insanityBar.fillAmount = insanityCurrent;

        // Green to Red
        insanityBar.color = new Color(insanityBar.fillAmount, 1 - insanityBar.fillAmount, 0);
    }

    private void UpdateIntoxicationBar()
    {
        intoxicationBar.fillAmount = intoxicationCurrent;

        // Green to Red
        intoxicationBar.color = new Color(intoxicationBar.fillAmount, 1 - intoxicationBar.fillAmount, 0);
    }

    private void UpdateThirstBar()
    {
        thirstBar.fillAmount = thirstCurrent;

        // Green to Red
        thirstBar.color = new Color(thirstBar.fillAmount, 1 - thirstBar.fillAmount, 0);
    }
}
