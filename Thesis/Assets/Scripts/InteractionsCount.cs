using NLog.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionsCount : MonoBehaviour
{
    public static int startedInteractionsFromExperimenterH1 = 0;
    public static int startedInteractionsFromMayorH1 = 0;
    public static int startedInteractionsFromWaitressH1 = 0;
    public static int startedInteractionsFromTesterH1 = 0;

    public static int startedInteractionsFromExperimenterH2 = 0;
    public static int startedInteractionsFromMayorH2 = 0;
    public static int startedInteractionsFromWaitressH2 = 0;
    public static int startedInteractionsFromTesterH2 = 0;

    public static int startedInteractionsFromExperimenterH3 = 0;
    public static int startedInteractionsFromMayorH3 = 0;
    public static int startedInteractionsFromWaitressH3 = 0;
    public static int startedInteractionsFromTesterH3 = 0;

    public static int startedInteractionsFromExperimenterH4 = 0;
    public static int startedInteractionsFromMayorH4 = 0;
    public static int startedInteractionsFromWaitressH4 = 0;
    public static int startedInteractionsFromTesterH4 = 0;

    public static int finishedInteractionsH1 = 0;
    public static int finishedInteractionsH2 = 0;
    public static int finishedInteractionsH3 = 0;
    public static int finishedInteractionsH4 = 0;

    private bool hasPrinted = false;

    void Awake()
    {
        hasPrinted = false;
    }

    private void Update()
    {
        if(hasPrinted == false)
        {
            NLogConfig.LogLine($"VERSION; Handshake 1");
            NLogConfig.LogLine($"VERSION; Interactions started from experimenter count: {startedInteractionsFromExperimenterH1}");
            NLogConfig.LogLine($"VERSION; Interactions started from Mayor count: {startedInteractionsFromMayorH1}");
            NLogConfig.LogLine($"VERSION; Interactions started from Waitress count: {startedInteractionsFromWaitressH1}");
            NLogConfig.LogLine($"VERSION; Interactions started from Tester count: {startedInteractionsFromTesterH1}");
            NLogConfig.LogLine($"VERSION; Finished interactions count (not counting interactions started from the Tester): {finishedInteractionsH1}");

            NLogConfig.LogLine("   ");

            NLogConfig.LogLine($"VERSION; Handshake 2");
            NLogConfig.LogLine($"VERSION; Started interactions from experimenter count: {startedInteractionsFromExperimenterH2}");
            NLogConfig.LogLine($"VERSION; Interactions started from Mayor count: {startedInteractionsFromMayorH2}");
            NLogConfig.LogLine($"VERSION; Interactions started from Waitress count: {startedInteractionsFromWaitressH2}");
            NLogConfig.LogLine($"VERSION; Interactions started from Tester count: {startedInteractionsFromTesterH2}");
            NLogConfig.LogLine($"VERSION; Finished interactions count (not counting interactions started from the Tester): {finishedInteractionsH2}");

            NLogConfig.LogLine("   ");

            NLogConfig.LogLine($"VERSION; Handshake 3");
            NLogConfig.LogLine($"VERSION; Started interactions from experimenter count: {startedInteractionsFromExperimenterH4}");
            NLogConfig.LogLine($"VERSION; Interactions started from Mayor count: {startedInteractionsFromMayorH4}");
            NLogConfig.LogLine($"VERSION; Interactions started from Waitress count: {startedInteractionsFromWaitressH4}");
            NLogConfig.LogLine($"VERSION; Interactions started from Tester count: {startedInteractionsFromTesterH4}");
            NLogConfig.LogLine($"VERSION; Finished interactions count (not counting interactions started from the Tester): {finishedInteractionsH4}");

            NLogConfig.LogLine("   ");

            NLogConfig.LogLine($"VERSION; Handshake 4");
            NLogConfig.LogLine($"VERSION; Started interactions from experimenter count: {startedInteractionsFromExperimenterH3}");
            NLogConfig.LogLine($"VERSION; Interactions started from Mayor count: {startedInteractionsFromMayorH3}");
            NLogConfig.LogLine($"VERSION; Interactions started from Waitress count: {startedInteractionsFromWaitressH3}");
            NLogConfig.LogLine($"VERSION; Interactions started from Tester count: {startedInteractionsFromTesterH3}");
            NLogConfig.LogLine($"VERSION; Finished interactions count (not counting interactions started from the Tester): {finishedInteractionsH3}");

            hasPrinted = true;
        }
    }
}
