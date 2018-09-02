using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class TrackController : MonoBehaviour
{
    public GameObject StartGo;
    public GameObject EndGo;
    public List<GameObject> checkPoints = new List<GameObject>();
    public float OffsetMultiplier = 0.3f;

    private GameObject nodePrefab;
    private GameObject cornerPrefab;

    public void Setup()
    {
        if (StartGo == null || EndGo == null) { return; }

        nodePrefab = Resources.Load<GameObject>("Prefabs/Race/RoadNode");
        cornerPrefab = Resources.Load<GameObject>("Prefabs/Race/DownRightCorner");

        var lastPoint = StartGo.transform.position;
        foreach (var checkPoint in checkPoints)
        {
            DrawRoad(lastPoint, checkPoint.transform.position);
            lastPoint = checkPoint.transform.position;
        }
        DrawRoad(lastPoint, EndGo.transform.position);

        var cornerList = new List<GameObject>();
        cornerList.Add(StartGo);
        cornerList.AddRange(checkPoints);
        cornerList.Add(EndGo);

        for (int i = 0; i < cornerList.Count - 2; i++)
        {
            DrawCorner(cornerList[i], cornerList[i + 2], cornerList[i + 1].transform.position);
        }
    }

    public void DestroyTrack()
    {
        var nodes = GetComponentsInChildren<RoadNode>();
        foreach (var node in nodes)
        {
            Destroy(node.gameObject);
        }
    }

    void DrawRoad(Vector3 startPos, Vector3 endPos)
    {
        var nodeGo = GameObject.Instantiate(nodePrefab, transform);
        var node = nodeGo.GetComponent<RoadNode>();

        var offset = (endPos - startPos).normalized * OffsetMultiplier;

        node.StartIndicator.transform.position = startPos + offset;
        node.EndIndicator.transform.position = endPos - offset;
        node.Setup();
    }

    void DrawCorner(GameObject prevPoint, GameObject nextPoint, Vector3 midPoint)
    {
        if (prevPoint == null || nextPoint == null) { return; }
        var corner = GameObject.Instantiate(cornerPrefab, transform);
        corner.transform.position = midPoint;

        var nextPos = nextPoint.transform.position;
        var prevPos = prevPoint.transform.position;

        var prevXDiff = (midPoint - prevPos).x;
        var prevZDiff = (midPoint - prevPos).z;
        var nextXDiff = (nextPos - midPoint).x;
        var nextZDiff = (nextPos - midPoint).z;

        if (Mathf.Abs(prevZDiff) > Mathf.Abs(prevXDiff))
        {
            // 세로 출발
            if (prevZDiff > 0)
            {
                // 아래
                if (nextXDiff > 0)
                {
                    // 오른쪽 끝        
                }
                else
                {
                    // 왼쪽 끝
                    corner.transform.localEulerAngles = new Vector3(0f, 90f, 0f);
                }
            }
            else
            {
                // 위
                if (nextXDiff > 0)
                {
                    // 오른쪽 끝
                    corner.transform.localEulerAngles = new Vector3(0f, -90f, 0f);
                }
                else
                {
                    // 왼쪽 끝
                    corner.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
                }
            }
        }
        else
        {
            // 가로 출발
            if (prevXDiff > 0)
            {
                // 왼쪽
                if (nextZDiff > 0)
                {
                    // 위쪽 끝
                    corner.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
                }
                else
                {
                    // 아래 끝
                    corner.transform.localEulerAngles = new Vector3(0f, 90f, 0f);
                }
            }
            else
            {
                // 오른쪽
                if (nextZDiff > 0)
                {
                    // 위쪽 끝
                    corner.transform.localEulerAngles = new Vector3(0f, -90f, 0f);
                }
                else
                {
                    // 아래 끝
                }
            }
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(TrackController))]
public class TrackControllerInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var trackController = (TrackController) target;

        GUI.enabled = Application.isPlaying;
        if(GUILayout.Button("맵 세팅"))
        {
            trackController.Setup();
        }

        if (GUILayout.Button("맵 파괴"))
        {
            trackController.DestroyTrack();
        }

        if (GUILayout.Button("레이스 시작"))
        {
            new StartRaceMsg().Dispatch();
        }
    }
}
#endif
