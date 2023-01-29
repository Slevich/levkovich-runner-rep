using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractStartButton : MonoBehaviour
{
    #region Fields
    [Header("The panel labelled 'Press E' at level HUD.")]
    [SerializeField] private GameObject pressEPanel;
    [Header("Red switch sprite.")]
    [SerializeField] private Sprite disactiveButtonSprite;
    [Header("Green switch sprite.")]
    [SerializeField] private Sprite activeButtonSprite;
    [Header("The component that controls the run timer.")]
    [SerializeField] private TimerController timerController;
    [Header("Component, that update top runs text on panel at start room.")]
    [SerializeField] private TopRunsPanelUpdater panelUpdater;
    [Header("Animator of the start room door.")]
    [SerializeField] private Animator doorAnimator;
    [Header("Variable indicating that the button is the start button, not the finish button.")]
    [SerializeField] private bool isButtonStartTimer;

    //LevelHUDManaher component on level canvas.
    private LevelHUDManager HUDManager;
    //InputActions class.
    private InputActions playerInput;
    //SpriteRenderer components of panel.
    private SpriteRenderer panelSR;
    //A variable indicating that the player is in the button trigger.
    private bool isPlayerNearby;
    #endregion

    #region Methods
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
    /// In Awake, we get the necessary components and classes.
    /// </summary>
    private void Awake()
    {
        playerInput = new InputActions();
        HUDManager = timerController.gameObject.GetComponent<LevelHUDManager>();
        panelSR = GetComponentInChildren<SpriteRenderer>();
    }

    /// <summary>
    /// At the start, timescale is reset to default.
    /// Sign up for the event by pressing the interaction button.
    /// </summary>
    private void Start()
    {
        Time.timeScale = 1;
        playerInput.Player.Interacting.started += Interacting_started;  
    }

    /// <summary>
    /// Event describing the behaviour for the start and finish buttons.
    /// At the start we simply change the sprite on the panel, 
    /// then pass a parameter to the door to open it.
    /// At the finish line, stop the time timer, 
    /// change the panel sprite, transmits to the HUD that the level is over. 
    /// Add a new run result to file.
    /// We stop the time in the game.
    /// </summary>
    /// <param name="obj"></param>
    private void Interacting_started(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (isButtonStartTimer && isPlayerNearby)
        {
            panelSR.sprite = disactiveButtonSprite;
            doorAnimator.SetBool("ButtonPressed", true);
        }
        else if (isButtonStartTimer == false && isPlayerNearby)
        {
            timerController.TimerStarted = false;
            panelSR.sprite = activeButtonSprite;
            HUDManager.LevelEnded = true;
            panelUpdater.AddNewRunToFile();
            Time.timeScale = 0;
        }
    }

    /// <summary>
    /// When the start button enters the trigger, 
    /// toggle state that the player is in the trigger, show the "Press E" panel.
    /// When entering the finish button trigger, similarly.
    /// </summary>
    /// <param name="collider"></param>
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player") && isButtonStartTimer && timerController.TimerStarted == false)
        {
            isPlayerNearby = true;
            pressEPanel.SetActive(true);
        }
        else if (collider.CompareTag("Player") && isButtonStartTimer == false)
        {
            isPlayerNearby = true;
            pressEPanel.SetActive(true);
        }
    }

    /// <summary>
    /// When exiting the trigger, remove the "Press E" panel. 
    /// Change the state that the player is not in the trigger.
    /// </summary>
    /// <param name="collider"></param>
    private void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            isPlayerNearby = false;
            pressEPanel.SetActive(false);
        }
    }
    #endregion
}
