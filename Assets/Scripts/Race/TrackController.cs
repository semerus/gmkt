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

    private GameObject nodePrefab;

    public void Setup()
    {
        if (StartGo == null || EndGo == null) { return; }

        nodePrefab = Resources.Load<GameObject>("Prefabs/Race/RoadNode");

        var lastPoint = StartGo.transform.position;
        foreach (var checkPoint in checkPoints)
        {
            DrawRoad(lastPoint, checkPoint.transform.position);
            lastPoint = checkPoint.transform.position;
        }
        DrawRoad(lastPoint, EndGo.transform.position);
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
        node.StartIndicator.transform.position = startPos;
        node.EndIndicator.transform.position = endPos;
        node.Setup();
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
