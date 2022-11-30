using NLog.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChangeLogButtons : MonoBehaviour
{
    [SerializeField] private InputActionReference _lines;
    [SerializeField] private InputActionReference _newTester;

    private void Start()
    {
        _lines.action.performed += ctx =>
        {
            NLogConfig.LogLine($"================================================================================");
        };

        _newTester.action.performed += ctx =>
        {
            NLogConfig.LogLine($"=============================NEW TESTER===================================");

            InteractionsCount.startedInteractionsFromExperimenterH1 = 0;
            InteractionsCount.startedInteractionsFromMayorH1 = 0;
            InteractionsCount.startedInteractionsFromWaitressH1 = 0;
            InteractionsCount.startedInteractionsFromTesterH1 = 0;


            InteractionsCount.finishedInteractionsWithExperimenterH1 = 0;
            InteractionsCount.finishedInteractionsWithMayorH1 = 0;
            InteractionsCount.finishedInteractionsWithWaitressH1 = 0;

            InteractionsCount.startedInteractionsFromExperimenterH2 = 0;
            InteractionsCount.startedInteractionsFromMayorH2 = 0;
            InteractionsCount.startedInteractionsFromWaitressH2 = 0;
            InteractionsCount.startedInteractionsFromTesterH2 = 0;

            InteractionsCount.finishedInteractionsWithExperimenterH2 = 0;
            InteractionsCount.finishedInteractionsWithMayorH2 = 0;
            InteractionsCount.finishedInteractionsWithWaitressH2 = 0;

            InteractionsCount.startedInteractionsFromExperimenterH3 = 0;
            InteractionsCount.startedInteractionsFromMayorH3 = 0;
            InteractionsCount.startedInteractionsFromWaitressH3 = 0;
            InteractionsCount.startedInteractionsFromTesterH3 = 0;

            InteractionsCount.finishedInteractionsWithExperimenterH3 = 0;
            InteractionsCount.finishedInteractionsWithMayorH3 = 0;
            InteractionsCount.finishedInteractionsWithWaitressH3 = 0;

            InteractionsCount.startedInteractionsFromExperimenterH4 = 0;
            InteractionsCount.startedInteractionsFromMayorH4 = 0;
            InteractionsCount.startedInteractionsFromWaitressH4 = 0;
            InteractionsCount.startedInteractionsFromTesterH4 = 0;

            InteractionsCount.finishedInteractionsWithExperimenterH4 = 0;
            InteractionsCount.finishedInteractionsWithMayorH4 = 0;
            InteractionsCount.finishedInteractionsWithWaitressH4 = 0;
        };
    }
}
