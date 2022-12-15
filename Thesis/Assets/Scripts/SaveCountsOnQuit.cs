using NLog.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveCountsOnQuit : MonoBehaviour
{
    private int sceneIndex;

    private void Start()
    {
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
    }
    private void OnApplicationQuit()
    {
        if(sceneIndex == 1)
        {
            InteractionsCount.exitInH1Time = System.DateTime.UtcNow;
        } else if(sceneIndex == 2)
        {
            InteractionsCount.exitInH2Time = System.DateTime.UtcNow;
        } else if(sceneIndex == 3)
        {
            InteractionsCount.exitInH3Time = System.DateTime.UtcNow;
        } else if(sceneIndex == 4)
        {
            InteractionsCount.exitInH4Time = System.DateTime.UtcNow;
        }        

        NLogConfig.LogLine($"================================================================================");
        InteractionsCount.PrintLogs();
    }
}
