using UnityEngine;

public class Barrier : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag != "Player") { return; }
        
        new BarrierCrashMsg().Dispatch();
    }
}
