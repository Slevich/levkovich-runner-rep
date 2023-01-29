using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerStarter : MonoBehaviour
{
    #region Field
    [Header("The component that controls the run timer.")]
    [SerializeField] private TimerController timerController;
    [Header("TextMeshPro component of the starter door.")]
    [SerializeField] private TextMeshPro startDoorText;
    #endregion

    #region Methods
    /// <summary>
    /// A method that starts the run timer and changes the text to "Go!".
    /// Called up at the door animation.
    /// </summary>
    private void StartTimer()
    {
        timerController.TimerStarted = true;
        startDoorText.text = "Go!";
    }
    #endregion
}
