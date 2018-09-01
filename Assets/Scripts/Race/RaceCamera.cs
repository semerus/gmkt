using UnityEngine;

public class RaceCamera : MonoBehaviour
{
    public GameObject LookTarget;

    private bool isInitialized;
    private Vector3 initPos;
    private Quaternion initQuaternion;

    void Awake()
    {
        initPos = transform.localPosition;
        initQuaternion = transform.localRotation;
        isInitialized = true;
    }

    public void ChangeToDeadMode()
    {
        transform.SetParent(transform.root);
    }

    public void ChangeToPlayingMode()
    {
        if(!isInitialized) { return; }
        transform.SetParent(LookTarget.transform);
        transform.localPosition = initPos;
        transform.localRotation = initQuaternion;
    }
}
