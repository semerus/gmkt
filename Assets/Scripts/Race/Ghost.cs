using DG.Tweening;
using GiraffeStar;
using UnityEngine;

public class Ghost : MonoBehaviour, ITimeHandler
{
    public float SpeedPerUnit;

    private Vector3 targetPosition;
    private TrackController trackController;
    private int currentCheckPointIndex;

    void Start()
    {
        var root = GameObject.Find("Root");
        trackController = root.FindChildByName("Track").GetComponent<TrackController>();
    }

    public void RunTime()
    {
    }

    public void Reset()
    {
        if (trackController == null)
        {
            var raceModule = GiraffeSystem.FindModule<RaceModule>();
            trackController = raceModule.TrackController;
        }
        transform.position = trackController.StartGo.transform.position;
    }

    public void SetLoose()
    {
        if (trackController.checkPoints.Count > 0)
        {
            currentCheckPointIndex = 0;
            targetPosition = trackController.checkPoints[0].transform.position;
        }
        else
        {
            // 바로 엔드 포지션으로 달려간다.
            targetPosition = trackController.EndGo.transform.position;
        }

        RunToTarget();
    }

    void RunToTarget()
    {
        var distance = Vector3.Distance(targetPosition, transform.position);
        var tween = transform.DOMove(targetPosition, distance * SpeedPerUnit);
        tween.OnComplete(() =>
        {
            if (currentCheckPointIndex < trackController.checkPoints.Count - 1)
            {
                // 다음 체크 포인트
                currentCheckPointIndex++;
                targetPosition = trackController.checkPoints[currentCheckPointIndex].transform.position;
                RunToTarget();
            }
            else if (currentCheckPointIndex == trackController.checkPoints.Count - 1)
            {
                // 마지막 포인트
                currentCheckPointIndex++;
                targetPosition = trackController.EndGo.transform.position;
                RunToTarget();
            }
            // 종료
        });
    }
}
