using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigatingTutorial : MonoBehaviour
{
    //Code responsible for handling the navugation through the tutorial

    private GameObject TutorialUI;

    private GameObject Page1UI;
    private GameObject Page2UI;
    private GameObject Page3UI;

    void Start()
    {
        TutorialUI = this.gameObject;
        Page1UI = TutorialUI.transform.GetChild(1).gameObject;
        Page2UI = TutorialUI.transform.GetChild(2).gameObject;
        Page3UI = TutorialUI.transform.GetChild(3).gameObject;
    }

    public void Next1()
    {
        Page2UI.SetActive(true);
        Page1UI.SetActive(false);
    }

    public void Next2()
    {
        Page3UI.SetActive(true);
        Page2UI.SetActive(false);
    }

    public void Back1()
    {
        Page1UI.SetActive(true);
        Page2UI.SetActive(false);
    }

    public void Back2()
    {
        Page2UI.SetActive(true);
        Page3UI.SetActive(false);
    }
}
