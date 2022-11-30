using NLog.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveCountsOnQuit : MonoBehaviour
{
    private void OnApplicationQuit()
    {
        NLogConfig.LogLine($"================================================================================");
        InteractionsCount.PrintLogs();
    }
}
