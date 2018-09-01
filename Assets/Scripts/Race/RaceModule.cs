using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GiraffeStar;

public class StartRaceMsg : MessageCore
{
}

public class BarrierCrashMsg : MessageCore
{
}

public class RaceModule : Module
{
    private bool isInitialized;

    private Race currentRace;
    private Car player;
    private Ghost ghost;
    private GameObject startPos;
    private GameObject endPos;

    [HideInInspector]
    public TrackController TrackController;

    public override void OnRegister()
    {
        base.OnRegister();
        Init();
    }

    void Init()
    {
        if(isInitialized) { return;}

        var root = GameObject.Find("Root");
        player = root.FindChildByName("Player").GetComponent<Car>();
        startPos = root.FindChildByName("StartPoint");
        endPos = root.FindChildByName("EndPoint");
        var endCollider = endPos.GetComponent<Collider2D>();
        TrackController = root.FindChildByName("Track").GetComponent<TrackController>();
        TrackController.Setup();
        ghost = root.FindChildByName("Ghost").GetComponent<Ghost>();

        StartRace();

        isInitialized = true;
    }

    void StartRace()
    {
        Reset();
        ghost.SetLoose();

        new PopupMsg()
        {
            Message = "Run For Your Life",
        }.Dispatch();
    }

    void Reset()
    {
        // 이부분도 나중에 플레이어로 옮겨놓기
        player.transform.position = startPos.transform.position.OverrideY(0.5f);
        player.transform.localEulerAngles = new Vector3(0f, 180f, 0f);

        ghost.Reset();
    }

    void EndRace()
    {
        Debug.Log("Race Ended");
        Reset();
    }

    [Subscriber]
    void OnCrashBarrier(BarrierCrashMsg msg)
    {
        EndRace();
    }

    [Subscriber]
    void OnStartRace(StartRaceMsg msg)
    {
        StartRace();
    }
}
