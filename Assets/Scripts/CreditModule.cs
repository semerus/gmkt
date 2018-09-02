using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GiraffeStar;
using UnityEngine;
using UnityEngine.UI;

public class ShowCreditSequenceMsg : MessageCore
{

}

public class CreditModule : Module
{
    private GameObject mainCredit;
    private GameObject mainCanvas;
    private GameObject targetPos;
    private Text timeText;

    private float runTime;

    public override void OnRegister()
    {
        var root = GameObject.Find("Root");
        mainCredit = root.FindChildByName("Credit");
        mainCanvas = mainCredit.FindChildByName("CreditCanvas");
        targetPos = mainCredit.FindChildByName("TargetPlace");
        timeText = mainCredit.FindChildByName("Timer").GetComponent<Text>();

        mainCredit.SetActive(true);
    }

    [Subscriber]
    void OnHandle(ShowCreditSequenceMsg msg)
    {
        PlayCreditScene();
    }

    void PlayCreditScene()
    {
        var raceModule = GiraffeSystem.FindModule<RaceModule>();
        var player = raceModule.Player;
        player.indicator.SleepEmote();
        player.StopCar();
        runTime = Time.timeSinceLevelLoad - raceModule.Timer;
        timeText.text = "Run Time : " + runTime.ToString();
        InputHandler.InputBlock = true;
        
        player.lastEnergy = Vector3.zero;

        player.transform.DOMove(targetPos.transform.position, 5f).OnComplete(() =>
            {
                
                player.transform.DORotate(targetPos.transform.localEulerAngles, 2f).OnComplete(() =>
                {
                    mainCanvas.SetActive(true);
                    DoCutscene();
                });
            });
    }

    void DoCutscene()
    {
        DOVirtual.DelayedCall(3f, () =>
        {
            new PopupMsg()
            {
                Message = "Wanna replay? It gets harder!",
                OnClick = () =>
                {
                    var raceModule = GiraffeSystem.FindModule<RaceModule>();
                    raceModule.IncreaseDifficulty();
                    new StartRaceMsg().Dispatch();
                },
                ButtonMessage = "Restart",
            }.Dispatch();
        });
    }

    public void Reset()
    {
        if (mainCanvas != null)
        {
            mainCanvas.SetActive(false);
        }
    }
}
