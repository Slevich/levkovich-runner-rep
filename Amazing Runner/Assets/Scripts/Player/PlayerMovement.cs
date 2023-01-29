using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region Fields
    [Header("Player movement speed.")]
    [SerializeField] private float movementSpeed;
    [Header("The speed of the player's turn when moving.")]
    [SerializeField] private float rotationSpeed;
    [Header("The number by which the player's sprint speed is multiplied.")]
    [SerializeField] private float sprintSpeedModifier;
    [Header("The number by which the player's crouch speed is multiplied.")]
    [SerializeField] private float crouchSpeedModifier;
    [Header("The force with which a player jumps.")]
    [SerializeField] private float jumpForce;
    [Header("Default Player Collider.")]
    [SerializeField] private GameObject playerDefaultCollider;
    [Header("A player's collider in crouch.")]
    [SerializeField] private GameObject playerCrouchCollider;

    //Transform of the main camera.
    private Transform cameraTransform;
    //The direction of the camera's gaze.
    private Vector3 cameraForwardDirection;
    //The direction of the player's movement.
    private Vector3 movingDirection;
    //Player's rigidbody.
    private Rigidbody playerRB;
    //Component with input.
    private InputActions playerInput;
    //The component responsible for player checks.
    private PlayerChecks playerChecks;
    //The component responsible for player animations.
    private PlayerAnimations playerAnim;

    //Limiting the player's speed for the animator.
    private float speedBorder = 0.25f;
    //The normal speed of a player when walking.
    private float defaultSpeed;
    //Default border of the player's movement speed for the animator.
    private float defaultBorder;
    //The length of the player's movement vector.
    private float movementVectorLength;

    //Variable responsible for whether the player is crouching.
    private bool isCrouching;
    //Variable responsible for whether the player jumps.
    private bool isJumping;
    #endregion

    #region Properties
    public float MovementVectorLength { get { return movementVectorLength; } }
    public bool IsCrouching { get { return isCrouching; } }
    public bool IsJumping { get { return isJumping; } set { isJumping = value; } }
    #endregion

    #region Methods
    /// <summary>
    /// The method checks for changes in the engine editor.
    /// </summary>
    private void OnValidate()
    {
        if (movementSpeed < 0)
        {
            movementSpeed = 0;
        }
        else if (rotationSpeed < 0)
        {
            rotationSpeed = 0;
        }
        else if (sprintSpeedModifier < 1)
        {
            sprintSpeedModifier = 1;
        }    
    }

    /// <summary>
    /// If the object is activated, the input is enable.
    /// </summary>
    private void OnEnable()
    {
        playerInput.Enable();
    }

    /// <summary>
    /// If the object is disactivated, the input is disable.
    /// </summary>
    private void OnDisable()
    {
        playerInput.Disable();
    }

    /// <summary>
    /// In Awake we get all the necessary components and classes.
    /// We get the transform of the main camera.
    /// We obtain the initial values of the speed variables.
    /// </summary>
    private void Awake()
    {
        playerInput = new InputActions();
        playerRB = GetComponent<Rigidbody>();
        playerChecks = GetComponent<PlayerChecks>();
        playerAnim = GetComponent<PlayerAnimations>();
        playerDefaultCollider.SetActive(true);
        cameraTransform = Camera.main.transform;
        defaultSpeed = movementSpeed;
        defaultBorder = speedBorder;
    }

    /// <summary>
    /// At the start we sign up for the events from the input system.
    /// </summary>
    private void Start()
    {
        playerInput.Player.Sprinting.performed += Sprinting_performed;
        playerInput.Player.Sprinting.canceled += Sprinting_canceled;
        playerInput.Player.Crouching.performed += Crouching_performed;
        playerInput.Player.Crouching.canceled += Crouching_canceled;
        playerInput.Player.Jumping.performed += Jumping_performed;
    }

    /// <summary>
    /// When you press the sprint button, 
    /// if there is no collider at the top, 
    /// the speed limit is increased to one (for the animator), 
    /// the movement speed is multiplied by the sprint modifier.
    /// </summary>
    /// <param name="obj"></param>
    private void Sprinting_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (playerChecks.IsColliderAbove == false)
        {
            speedBorder = 1;
            movementSpeed = defaultSpeed * sprintSpeedModifier;
        }
    }

    /// <summary>
    /// When you press the sprint button, 
    /// if there is no collider at the top, 
    /// the speed limit and the speed itself return to the default.
    /// </summary>
    /// <param name="obj"></param>
    private void Sprinting_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (playerChecks.IsColliderAbove == false)
        {
            speedBorder = defaultBorder;
            movementSpeed = defaultSpeed;
        }
    }

    /// <summary>
    /// If the player is not in a jump, change the state to a crouch, 
    /// change the colliders, and multiply the movement speed by the modifier.
    /// </summary>
    /// <param name="obj"></param>
    private void Crouching_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (isJumping == false)
        {
            isCrouching = true;
            playerDefaultCollider.SetActive(false);
            playerCrouchCollider.SetActive(true);
            movementSpeed = defaultSpeed * crouchSpeedModifier;
        }
    }

    /// <summary>
    /// If there is no collider above the player, 
    /// then change the player's colliders, switch to a non-sitting state.
    /// </summary>
    /// <param name="obj"></param>
    private void Crouching_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (playerChecks.IsColliderAbove == false)
        {
            playerCrouchCollider.SetActive(false);
            playerDefaultCollider.SetActive(true);
            isCrouching = false;
        }
    }

    /// <summary>
    /// If the player is no longer in a jump and is on the ground, change the state to in a jump.
    /// </summary>
    /// <param name="obj"></param>
    private void Jumping_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (isJumping == false && playerChecks.OnGround)
        {
            isJumping = true;
        }
    }

    /// <summary>
    /// In Update we call the methods that take the player out of the crouch.
    /// Update the camera gaze direction.
    /// Rotate the character.
    /// </summary>
    private void Update()
    {
        CharacterUnCrouch();
        UpdateCameraForwardDirection();
        RotateCharacter(movingDirection);
    }

    /// <summary>
    /// In FixedUpdate we get the axis point of input from the keyboard.
    /// We obtain the length of the motion vector.
    /// Next, we move the character.
    /// </summary>
    private void FixedUpdate()
    {
        Vector2 inputAxisDirection = playerInput.Player.Movement.ReadValue<Vector2>();
        movementVectorLength = Vector3.ClampMagnitude(movingDirection, speedBorder).magnitude;

        if (playerChecks.OnGround == false && isJumping == false) MainCharacteIsFalling();
        else if (isJumping == false) MoveCharacterOnGround(cameraForwardDirection, inputAxisDirection);
    }

    /// <summary>
    /// The method updates the direction of the camera view.
    /// </summary>
    private void UpdateCameraForwardDirection() => cameraForwardDirection = Vector3.Scale(cameraTransform.forward, 
                                                                                          new Vector3(1, 0, 1)).normalized;
    /// <summary>
    /// The method moves the player in the direction of the camera view.
    /// </summary>
    /// <param name="cameraForwardDirection"></param>
    /// <param name="inputAxisDirection"></param>
    private void MoveCharacterOnGround(Vector3 cameraForwardDirection, Vector2 inputAxisDirection)
    {
        movingDirection = inputAxisDirection.y * cameraForwardDirection + inputAxisDirection.x * cameraTransform.right;
        playerRB.velocity = movingDirection * movementSpeed;
    }

    /// <summary>
    /// Method moves player down.
    /// </summary>
    private void MainCharacteIsFalling()
    {
        playerRB.velocity = Vector3.down * movementSpeed;
    }

    /// <summary>
    /// The method turns the player in the direction of the camera's gaze.
    /// </summary>
    /// <param name="movingDirection"></param>
    private void RotateCharacter(Vector3 movingDirection)
    {
        if (movingDirection.magnitude > Mathf.Abs(0.01f))
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(movingDirection), Time.deltaTime * rotationSpeed);
        }
        else
        {
            playerRB.angularVelocity = Vector3.zero;
        }
    }

    /// <summary>
    /// The method switches the player's colliders.
    /// Turns off the crouch.
    /// </summary>
    private void CharacterUnCrouch()
    {
        if (playerChecks.IsColliderAbove == false && isCrouching && playerInput.Player.Crouching.inProgress == false)
        {
            playerCrouchCollider.SetActive(false);
            playerDefaultCollider.SetActive(true);
            isCrouching = false;
        }
    }

    /// <summary>
    /// The method gives new velocity to players RigidBody with .
    /// </summary>
    private void AddJumpForce()
    {
        playerRB.velocity = new Vector3(movingDirection.x, 1, movingDirection.z) * jumpForce;
    }

    /// <summary>
    /// The method stops the jump.
    ///Called at the end of the landing animation.
    /// </summary>
    private void StopJumping() => IsJumping = false;
    #endregion
}
