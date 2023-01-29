using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerUIUpdater : MonoBehaviour
{
    #region Fields
    [Header("TextMeshPro component with the number of minutes of the run.")]
    [SerializeField] private TextMeshProUGUI timerMinutesText;
    [Header("TextMeshPro component with the number of seconds of the run.")]
    [SerializeField] private TextMeshProUGUI timerSecondsText;

    //The component that controls the run timer.
    private TimerController timerController;
    #endregion

    #region Methods
    /// <summary>
    /// First we get the necessary component.
    /// We zero out the texts.
    /// </summary>
    private void Awake()
    {
        timerController = GetComponent<TimerController>();
        timerMinutesText.text = "0";
        timerSecondsText.text = "0";
    }

    /// <summary>
    /// In Update we call the text update.
    /// </summary>
    private void Update()
    {
        UpdateTimerTexts();
    }

    /// <summary>
    /// Method pass in the text fields, the number of minutes and seconds of the race.
    /// </summary>
    private void UpdateTimerTexts()
    {
        if (timerController.TimerStarted)
        {
            timerMinutesText.text = timerController.TimerMinutes.ToString();
            timerSecondsText.text = timerController.TimerSeconds.ToString();
        }
    }
    #endregion
}
