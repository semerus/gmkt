using GiraffeStar;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class RoadNode : MonoBehaviour
{
    public GameObject StartIndicator;
    public GameObject EndIndicator;

    private Vector3 startPos;
    private Vector3 endPos;
    private GameObject plane;
    private GameObject leftBarrier;
    private GameObject rightBarrier;

    void Awake()
    {
        plane = gameObject.FindChildByName("Plane");
        leftBarrier = gameObject.FindChildByName("LeftBarrier");
        rightBarrier = gameObject.FindChildByName("RightBarrier");
    }

    public void Setup()
    {
        if (plane == null)
        {
            Awake();
        }

        if (StartIndicator != null)
        {
            startPos = StartIndicator.transform.position;
        }

        if (EndIndicator != null)
        {
            endPos = EndIndicator.transform.position;
        }

        var distance = Vector3.Distance(startPos, endPos);
        var midPoint = (startPos + endPos) / 2f;
        //transform.position = midPoint;
        //StartIndicator.transform.position = startPos;
        //EndIndicator.transform.position = endPos;

        plane.transform.position = midPoint.OverrideY(0.1f);
        plane.transform.localScale = plane.transform.localScale.OverrideZ(distance * 0.1f);

        var angle = 0f;

        if (startPos.x > endPos.x)
        {
            angle = Vector2.Angle(Vector2.up, Convert(startPos) - Convert(midPoint));
        }
        else
        {
            angle = Vector2.Angle(Vector2.up, Convert(endPos) - Convert(midPoint));
        }

        if (angle > 90f)
        {
            angle -= 180f;
        }

        var planeRotation = new Vector3(0f, angle, 0f);
        plane.transform.localEulerAngles = planeRotation;

        // adjust barriers
        leftBarrier.transform.position = midPoint;
        leftBarrier.transform.localEulerAngles = planeRotation;
        leftBarrier.transform.localScale =
            leftBarrier.transform.localScale.OverrideZ(plane.transform.localScale.z * 10f);

        rightBarrier.transform.localEulerAngles = planeRotation;
        rightBarrier.transform.position = midPoint;
        rightBarrier.transform.localScale =
            rightBarrier.transform.localScale.OverrideZ(plane.transform.localScale.z * 10f);
    }

    static Vector2 Convert(Vector3 vector)
    {
        return new Vector2(vector.x, vector.z);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(RoadNode))]
public class RoadNodeInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var roadNode = (RoadNode)target;

        if (GUILayout.Button("위치 적용"))
        {
            roadNode.Setup();
        }
    }
}

#endif


