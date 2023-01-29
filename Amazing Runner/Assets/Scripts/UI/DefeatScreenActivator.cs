using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefeatScreenActivator : MonoBehaviour
{
    #region Fields
    [Header("Game object with a screen on the level.")]
    [SerializeField] private GameObject playableScreen;
    [Header("Game object with a screen on the level.")]
    [SerializeField] private GameObject defeatScreen;
    #endregion

    #region Methods
    /// <summary>
    /// If player enter the trigger,
    /// disactivate level HUD and
    /// activate defeat screen.
    /// </summary>
    /// <param name="collider"></param>
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            playableScreen.SetActive(false);
            defeatScreen.SetActive(true);
            Time.timeScale = 0;
        }
    }
    #endregion
}
