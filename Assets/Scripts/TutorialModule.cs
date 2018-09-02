using System.Collections;
using System.Collections.Generic;
using GiraffeStar;
using UnityEngine;

public class TutorialModule : Module
{
    private InputHandler inputHandler;
    private GameObject tutorialMain;
    private GameObject tutorialText;

    public override void OnRegister()
    {
        base.OnRegister();

        var root = GameObject.Find("Root");
        tutorialMain = root.FindChildByName("Tutorial");
        tutorialText = tutorialMain.FindChildByName("Text");

        tutorialMain.SetActive(true);
        tutorialText.SetActive(false);

        CheckFirstTime();
    }

    void CheckFirstTime()
    {
        var record = PlayerPrefs.GetString("TutorialDone", "False");
        //if (record.Equals("True")) { return; }

        var root = GameObject.Find("Root");
        inputHandler = root.FindChildByName("Player").GetComponent<InputHandler>();
        inputHandler.OnAnyKeyDown = ShowTutorial;
    }

    void ShowTutorial()
    {
        PlayerPrefs.SetString("TutorialDone", "True");

        inputHandler.OnAnyKeyDown = EndTutorial;
        tutorialText.SetActive(true);
    }

    void EndTutorial()
    {
        tutorialMain.SetActive(false);
        inputHandler.OnAnyKeyDown = null;
        new StartRaceMsg().Dispatch();
    }
}
