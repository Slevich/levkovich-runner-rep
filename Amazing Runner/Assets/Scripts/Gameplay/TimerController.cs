using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerController : MonoBehaviour
{
    #region Fields
    //Variable indicating whether the timer is running or not.
    private bool timerStarted;
    //Total run timer (seconds).
    private float runTimer;
    //Seconds of the run timer.
    private int timerSeconds;
    //Minutes of the run timer.
    private int timerMinutes;
    #endregion

    #region Properties
    public bool TimerStarted { get { return timerStarted; } set { timerStarted = value; } }
    public int TimerSeconds { get { return timerSeconds; } }
    public int TimerMinutes { get { return timerMinutes; } }
    #endregion

    #region Methods
    /// <summary>
    /// On awakening, the run timer is reset to zero.
    /// </summary>
    private void Awake()
    {
        runTimer = 0;
    }

    /// <summary>
    /// In Update, run the method with the run timer.
    /// </summary>
    private void Update()
    {
        RunTimer();
    }

    /// <summary>
    /// If the timer is running, add the current DeltaTime to the timer.
    /// Add whole numbers of seconds to the seconds.
    ///Convert seconds to minutes.
    /// </summary>
    private void RunTimer()
    {
        if (timerStarted)
        {
            runTimer += Time.deltaTime;
            timerSeconds = Mathf.FloorToInt(runTimer);
            ConvertSecondsToMinutes();
        }
    }

    /// <summary>
    /// If the number of seconds is greater than or equal to 60, 
    /// add one to the minutes and subtract 60 from the timer.
    /// </summary>
    private void ConvertSecondsToMinutes()
    {
        if (runTimer >= 60)
        {
            timerMinutes++;
            runTimer -= 60;
        }
    }
    #endregion
}
