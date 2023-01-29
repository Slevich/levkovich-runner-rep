using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChecks : MonoBehaviour
{
    #region Fields
    [Header("LayerMask 'Ground'.")]
    [SerializeField] private LayerMask groundMask;
    [Header("RayCast starting Point.")]
    [SerializeField] private Transform raycastStartPoint;
    [Header("Ray length to see if the player can stand up.")]
    [SerializeField] private float headRayDistance;
    [Header("Ray length to check if the player is on the ground.")]
    [SerializeField] private float groundRayDistance;

    //Variable indicating whether there is a collider over the player's head.
    private bool isColliderAbove;
    //Variable indicating whether the player is on the ground.
    private bool onGround;
    //The component that controls the player's movement.
    private PlayerMovement playerMovement;
    #endregion

    #region Properties
    public bool IsColliderAbove { get { return isColliderAbove; } }
    public bool OnGround { get { return onGround; } }
    #endregion

    #region Methods
    /// <summary>
    /// In Awake we get the necessary component.
    /// </summary>
    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    /// <summary>
    /// In Update draws lines for debugging.
    /// We check if the player is on the ground.
    /// If the player is in a crouch, 
    /// check to see if there is a collider over his head.
    /// </summary>
    private void Update()
    {
        Debug.DrawLine(raycastStartPoint.position, new Vector3(raycastStartPoint.position.x, 
                                                               raycastStartPoint.position.y - groundRayDistance, 
                                                               raycastStartPoint.position.z), 
                                                               Color.green);
        Debug.DrawLine(raycastStartPoint.position, new Vector3(raycastStartPoint.position.x,
                                                               raycastStartPoint.position.y + headRayDistance, 
                                                               raycastStartPoint.position.z), 
                                                               Color.red);

        CheckGround();

        if (playerMovement.IsCrouching)
        {
            CheckUpper();
        }
    }

    /// <summary>
    /// The method cast a ray, to check if there is a collider with a ground mask under the player's feet.
    /// </summary>
    private void CheckGround() => onGround = Physics.Raycast(raycastStartPoint.position, Vector3.down, groundRayDistance, groundMask);

    /// <summary>
    /// The method checks if there is a collider with a ground mask over the player's head.
    /// </summary>
    private void CheckUpper() => isColliderAbove = Physics.Raycast(raycastStartPoint.position, Vector3.up, headRayDistance, groundMask);
    #endregion
}
