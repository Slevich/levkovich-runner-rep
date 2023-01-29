using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelHUDManager : MonoBehaviour
{
    #region Fields
    [Header("Game object with a screen on the level.")]
    [SerializeField] private GameObject playableScreen;
    [Header("Game object with a screen on the level.")]
    [SerializeField] private GameObject finishScreen;
    [Header("TextMeshPro on GUI containing text with the number of minutes of the run.")]
    [SerializeField] private TextMeshProUGUI resultMinutesText;
    [Header("TextMeshPro on GUI containing text with the number of seconds of the run.")]
    [SerializeField] private TextMeshProUGUI resultSecondsText;

    //The component that controls the run timer.
    private TimerController timerController;
    //Variable indicating whether the level is complete or not.
    private bool levelEnded;
    #endregion

    #region Properties
    public bool LevelEnded { set { levelEnded = value; } get { return levelEnded; } }
    #endregion

    #region Methods
    /// <summary>
    /// At the start, get the needed component.
    /// </summary>
    private void Awake()
    {
        timerController = GetComponent<TimerController>();
    }

    /// <summary>
    /// In Update check whether the level is finished.
    /// If so, turn off the game screen and turn on the finish screen.
    /// Update the result on the Finish screen.
    /// </summary>
    private void Update()
    {
        if (levelEnded)
        {
            playableScreen.SetActive(false);
            finishScreen.SetActive(true);
            resultMinutesText.text = timerController.TimerMinutes.ToString();
            resultSecondsText.text = timerController.TimerSeconds.ToString();
        }
    }

    /// <summary>
    /// The method restarts the current level.
    /// </summary>
    public void RetryLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    #endregion
}
