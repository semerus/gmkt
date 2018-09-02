using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using GiraffeStar;

public class StartRaceMsg : MessageCore
{
}

public class BarrierCrashMsg : MessageCore
{
}

public class EatenByGhostMsg : MessageCore
{
}

public class RaceModule : Module
{
    private bool isInitialized;

    private Race currentRace;
    public Car Player { get; private set; }
    private Ghost ghost;
    private GameObject startPos;
    private GameObject endPos;

    [HideInInspector]
    public TrackController TrackController;

    public float Timer;

    public override void OnRegister()
    {
        base.OnRegister();
        Init();
    }

    void Init()
    {
        if(isInitialized) { return;}

        var root = GameObject.Find("Root");
        Player = root.FindChildByName("Player").GetComponent<Car>();
        startPos = root.FindChildByName("StartPoint");
        endPos = root.FindChildByName("EndPoint");
        var endCollider = endPos.GetComponent<Collider2D>();
        TrackController = root.FindChildByName("Track").GetComponent<TrackController>();
        TrackController.Setup();
        ghost = root.FindChildByName("Ghost").GetComponent<Ghost>();

        //StartRace();

        isInitialized = true;
    }

    void StartRace()
    {
        Reset();
        Player.StartCar();

        Timer = Time.timeSinceLevelLoad;

        DOVirtual.DelayedCall(0.2f, () =>
        {
            new PopupMsg()
            {
                Message = "Run For Your Life Hank!!",
            }.Dispatch();
        });

        DOVirtual.DelayedCall(3f, () => { ghost.SetLoose(); });
    }

    void Reset()
    {
        // 이부분도 나중에 플레이어로 옮겨놓기
        Player.transform.position = startPos.transform.position.OverrideY(0.5f);
        Player.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
        Player.gameObject.SetActive(true);

        ghost.Reset();

        foreach (var pad in TrackController.BoosterPads)
        {
            pad.Reset();
        }

        InputHandler.InputBlock = false;

        var creditModule = GiraffeSystem.FindModule<CreditModule>();
        if (creditModule != null)
        {
            creditModule.Reset();
        }
    }

    void EndRace()
    {
        Debug.Log("Race Ended");
        //Reset();
        Player.StopCar();
        ghost.StopGhost();
    }

    [Subscriber]
    void OnCrashBarrier(BarrierCrashMsg msg)
    {
        EndRace();

        new PopupMsg()
        {
            Message = "You crashed. Click Button to restart",
            ButtonMessage = "Restart",
            OnClick = StartRace,
        }.Dispatch();
    }

    [Subscriber]
    void OnEatenByGhost(EatenByGhostMsg msg)
    {
        EndRace();
        Player.playerCamera.ChangeToDeadMode();
        
        Player.gameObject.SetActive(false);

        new PopupMsg()
        {
            Message = "Hank met his doom ㅜ_ㅜ",
            ButtonMessage = "Restart",
            OnClick = StartRace,
        }.Dispatch();
    }

    [Subscriber]
    void OnStartRace(StartRaceMsg msg)
    {
        StartRace();
    }

    public void IncreaseDifficulty()
    {
        ghost.SpeedIncreaser = ghost.SpeedIncreaser * 0.99f;
    }
}
