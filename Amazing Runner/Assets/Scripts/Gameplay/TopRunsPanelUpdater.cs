using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;

public class TopRunsPanelUpdater : MonoBehaviour
{
    #region Fields
    [Header("TextMeshPro with top runs text.")]
    [SerializeField] private TextMeshPro topRunsText;
    [Header("TextMeshPro with top runs text.")]
    [SerializeField] private LevelHUDManager HUDManager;
    [Header("Component, which controlls run timer.")]
    [SerializeField] private TimerController timerController;

    //String with path of file "TopRuns.txt".
    private string topRunsFilePath; //File located at: Assets/SpecificFiles.
    //String with text of file "TopRuns.txt".
    private string fileText;
    #endregion

    #region Methods
    /// <summary>
    /// At the start we get the file path.
    /// We get the whole text of the file.
    /// Transfers the file text to the text in the panel.
    /// </summary>
    private void Start()
    {
        GetTopRunsFilePath();
        GetTextFromFile();
        topRunsText.text = fileText;
    }

    /// <summary>
    /// The method gets the path to the file with the top runs.
    /// </summary>
    private void GetTopRunsFilePath()
    {
        topRunsFilePath = $@"{Application.dataPath}/SpecificFiles/TopRuns.txt";
    }

    /// <summary>
    /// We get the whole text of the file.
    /// </summary>
    private void GetTextFromFile()
    {
        using (StreamReader fileReader = new StreamReader(topRunsFilePath))
        {
            fileText = fileReader.ReadToEnd();
        }
    }

    /// <summary>
    /// The method overwrites the file and adds a line with the new result.
    /// </summary>
    public void AddNewRunToFile()
    {
        string newRunString = $"{timerController.TimerMinutes} : {timerController.TimerSeconds}";
        using (StreamWriter fileWriter = new StreamWriter(topRunsFilePath))
        {
            fileWriter.Write(fileText);
            fileWriter.WriteLine(newRunString);
        }
    }
    #endregion
}
