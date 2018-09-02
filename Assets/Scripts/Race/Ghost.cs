using DG.Tweening;
using GiraffeStar;
using UnityEngine;

public class Ghost : MonoBehaviour, ITimeHandler
{
    public float SpeedPerUnit;

    private Vector3 targetPosition;
    private TrackController trackController;
    private int currentCheckPointIndex;
    private Tween movingTween;

    // audio
    private AudioSource crying;
    private AudioSource moving;

    void Start()
    {
        var root = GameObject.Find("Root");
        trackController = root.FindChildByName("Track").GetComponent<TrackController>();
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            new EatenByGhostMsg().Dispatch();
        }
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

        if (movingTween != null)
        {
            movingTween.Kill();
        }
        currentCheckPointIndex = 0;
        transform.position = trackController.StartGo.transform.position;
        gameObject.SetActive(false);

        // audio
        crying = gameObject.FindChildByName("Cry").GetComponent<AudioSource>();
        moving = gameObject.FindChildByName("Moving").GetComponent<AudioSource>();
    }

    public void SetLoose()
    {
        gameObject.SetActive(true);
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

        ControlSounds(crying, true);
        RunToTarget();
    }

    void RunToTarget()
    {
        transform.LookAt(targetPosition);
        var distance = Vector3.Distance(targetPosition, transform.position);
        movingTween = transform.DOMove(targetPosition, distance * SpeedPerUnit);
        movingTween.OnComplete(() =>
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

        ControlSounds(moving, true);
    }

    public void StopGhost()
    {
        if (movingTween != null)
        {
            movingTween.Kill();
        }

        ControlSounds(moving, false);
    }

    public void ControlSounds(AudioSource audio, bool play)
    {
        if (audio == null)
        {
            return;
        }

        if (play)
        {
            if(audio.isPlaying) { return;}
            audio.Play();
        }
        else
        {
            audio.Stop();
        }
    }
}
