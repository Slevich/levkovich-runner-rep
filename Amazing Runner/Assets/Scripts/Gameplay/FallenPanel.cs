using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallenPanel : MonoBehaviour
{
    #region Fields
    [Header("The time it takes for the panel to drop when stepping on it.")]
    [SerializeField] private float waitingTimer;

    //Variable denoting whether the player has hit the panel trigger.
    private bool isPlayerOnPanel;
    //Rigidbody of panel's object.
    private Rigidbody panelRB;
    #endregion

    #region Methods
    /// <summary>
    /// Get panel's rigidbody on start.
    /// </summary>
    private void Awake()
    {
        panelRB = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// If the player hits the trigger, 
    /// start a timer, 
    /// after which the gravity is activated.
    /// </summary>
    private void Update()
    {
        if (isPlayerOnPanel)
        {
            waitingTimer -= Time.deltaTime;

            if (waitingTimer <= 0)
            {
                panelRB.useGravity = true;
            }
        }
    }

    /// <summary>
    /// When the trigger is entered, the variable is toggled.
    /// </summary>
    /// <param name="collider"></param>
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            isPlayerOnPanel = true;
        }
    }
    #endregion
}
