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
    private GameObject startPos;
    private GameObject endPos;
    private TrackController trackController;

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

        trackController = root.FindChildByName("Track").GetComponent<TrackController>();
        trackController.Setup();

        StartRace();

        isInitialized = true;
    }

    void StartRace()
    {
        Reset();
    }

    void Reset()
    {
        player.transform.position = startPos.transform.position.OverrideY(0.5f);
        player.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
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
