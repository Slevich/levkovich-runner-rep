using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    #region Fields
    //The component that controls the player's movement.
    private PlayerMovement playerMovement;
    //The component responsible for player checks.
    private PlayerChecks playerChecks;
    //Player's animator.
    private Animator playerAnim;

    private float jumpPosition;
    #endregion

    #region Properties
    public float JumpPosition { get { return jumpPosition; } }
    #endregion

    #region Methods
    /// <summary>
    /// In Awake we get the necessary components.
    /// </summary>
    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerChecks = GetComponent<PlayerChecks>();
        playerAnim = GetComponent<Animator>();
    }

    /// <summary>
    /// In Update we call methods to change the parameters of the animator.
    /// </summary>
    private void Update()
    {
        jumpPosition = playerAnim.GetFloat("JumpPosition");
        Debug.Log(jumpPosition);
        UpdateAnimatorParameters();
        UpdateAnimatorJumpingParameters();
    }

    /// <summary>
    /// The method passes parameters to the animator.
    /// </summary>
    private void UpdateAnimatorParameters()
    {
        playerAnim.SetFloat("Speed", playerMovement.MovementVectorLength);
        playerAnim.SetBool("IsCrouch", playerMovement.IsCrouching);
    }

    /// <summary>
    /// The method changes the jump position, 
    /// depending on whether the player jumps and is on the ground.
    /// </summary>
    private void UpdateAnimatorJumpingParameters()
    {
        if (playerMovement.IsJumping && playerChecks.OnGround)
        {
            playerAnim.SetBool("IsJumping", true);
            if (playerAnim.GetFloat("JumpPosition") == 0.5f) ChangeJumpPosition(1f);
        }
        else if (playerMovement.IsJumping == false && playerChecks.OnGround)
        {
            playerAnim.SetBool("IsJumping", false);
            ChangeJumpPosition(0f);
        }
        else if (playerMovement.IsJumping == false && playerChecks.OnGround == false)
        {
            playerAnim.SetBool("IsJumping", true);
            ChangeJumpPosition(0.5f);
        }
    }

    /// <summary>
    /// The method changes the jump position parameter in the animator.
    /// </summary>
    /// <param name="jumpPosition"></param>
    private void ChangeJumpPosition(float jumpPosition) => playerAnim.SetFloat("JumpPosition", jumpPosition);
    #endregion
}
