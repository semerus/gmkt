using UnityEngine;
using GiraffeStar;

public class FirstAwake : MonoBehaviour {

    void Awake()
    {
        InitialExecution.Init();
        gameObject.AddComponent<TimeSystem>();


        // 임시!!!
    }
}
