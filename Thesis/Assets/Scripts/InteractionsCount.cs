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

    public static int finishedInteractionsWithExperimenterH1 = 0;
    public static int finishedInteractionsWithMayorH1 = 0;
    public static int finishedInteractionsWithWaitressH1 = 0;

    public static int finishedInteractionsWithExperimenterH2 = 0;
    public static int finishedInteractionsWithMayorH2 = 0;
    public static int finishedInteractionsWithWaitressH2 = 0;

    public static int finishedInteractionsWithExperimenterH3 = 0;
    public static int finishedInteractionsWithMayorH3 = 0;
    public static int finishedInteractionsWithWaitressH3 = 0;

    public static int finishedInteractionsWithExperimenterH4 = 0;
    public static int finishedInteractionsWithMayorH4 = 0;
    public static int finishedInteractionsWithWaitressH4 = 0;

    //public static int finishedInteractionsH1 = 0;
    //public static int finishedInteractionsH2 = 0;
    //public static int finishedInteractionsH3 = 0;
    //public static int finishedInteractionsH4 = 0;

    private bool hasPrinted = false;

    void Awake()
    {
        hasPrinted = false;
    }

    private void Update()
    {
        if(hasPrinted == false)
        {
            PrintLogs();

            hasPrinted = true;
        }
    }

    private void OnApplicationQuit()
    {
        PrintLogs();
    }

    public static void PrintLogs()
    {
        NLogConfig.LogLine($"VERSION; Handshake 1");
        NLogConfig.LogLine($"Interactions started from experimenter count; {startedInteractionsFromExperimenterH1}");
        NLogConfig.LogLine($"Interactions started from Mayor count; {startedInteractionsFromMayorH1}");
        NLogConfig.LogLine($"Interactions started from Waitress count; {startedInteractionsFromWaitressH1}");
        NLogConfig.LogLine($"Interactions started from Tester count; {startedInteractionsFromTesterH1}");
        NLogConfig.LogLine($"Finished interactions with experimenter count; {finishedInteractionsWithExperimenterH1}");
        NLogConfig.LogLine($"Finished interactions with Mayor count; {finishedInteractionsWithMayorH1}");
        NLogConfig.LogLine($"Finished interactions with Waitress count; {finishedInteractionsWithWaitressH1}");
        //NLogConfig.LogLine($"Finished interactions count (not counting interactions started from the Tester); {finishedInteractionsH1}");

        NLogConfig.LogLine("   ");

        NLogConfig.LogLine($"VERSION; Handshake 2");
        NLogConfig.LogLine($"Interactions started from experimenter count; {startedInteractionsFromExperimenterH2}");
        NLogConfig.LogLine($"Interactions started from Mayor count; {startedInteractionsFromMayorH2}");
        NLogConfig.LogLine($"Interactions started from Waitress count; {startedInteractionsFromWaitressH2}");
        NLogConfig.LogLine($"Interactions started from Tester count; {startedInteractionsFromTesterH2}");
        NLogConfig.LogLine($"Finished interactions with experimenter count; {finishedInteractionsWithExperimenterH2}");
        NLogConfig.LogLine($"Finished interactions with Mayor count; {finishedInteractionsWithMayorH2}");
        NLogConfig.LogLine($"Finished interactions with Waitress count; {finishedInteractionsWithWaitressH2}");
        //NLogConfig.LogLine($"Finished interactions count (not counting interactions started from the Tester); {finishedInteractionsH2}");

        NLogConfig.LogLine("   ");

        NLogConfig.LogLine($"VERSION; Handshake 3");
        NLogConfig.LogLine($"Interactions started from experimenter count; {startedInteractionsFromExperimenterH4}");
        NLogConfig.LogLine($"Interactions started from Mayor count; {startedInteractionsFromMayorH4}");
        NLogConfig.LogLine($"Interactions started from Waitress count; {startedInteractionsFromWaitressH4}");
        NLogConfig.LogLine($"Interactions started from Tester count; {startedInteractionsFromTesterH4}");
        NLogConfig.LogLine($"Finished interactions with experimenter count; {finishedInteractionsWithExperimenterH4}");
        NLogConfig.LogLine($"Finished interactions with Mayor count; {finishedInteractionsWithMayorH4}");
        NLogConfig.LogLine($"Finished interactions with Waitress count; {finishedInteractionsWithWaitressH4}");
        //NLogConfig.LogLine($"Finished interactions count (not counting interactions started from the Tester); {finishedInteractionsH4}");

        NLogConfig.LogLine("   ");

        NLogConfig.LogLine($"VERSION; Handshake 4");
        NLogConfig.LogLine($"Interactions started from experimenter count; {startedInteractionsFromExperimenterH3}");
        NLogConfig.LogLine($"Interactions started from Mayor count; {startedInteractionsFromMayorH3}");
        NLogConfig.LogLine($"Interactions started from Waitress count; {startedInteractionsFromWaitressH3}");
        NLogConfig.LogLine($"Interactions started from Tester count; {startedInteractionsFromTesterH3}");
        NLogConfig.LogLine($"Finished interactions with experimenter count; {finishedInteractionsWithExperimenterH3}");
        NLogConfig.LogLine($"Finished interactions with Mayor count; {finishedInteractionsWithMayorH3}");
        NLogConfig.LogLine($"Finished interactions with Waitress count; {finishedInteractionsWithWaitressH3}");
        //NLogConfig.LogLine($"Finished interactions count (not counting interactions started from the Tester); {finishedInteractionsH3}");
    }
}
