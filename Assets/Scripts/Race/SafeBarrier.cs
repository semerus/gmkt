using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeBarrier : MonoBehaviour {

    void OnCollisionStay(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            var car = collision.transform.GetComponent<Car>();
            car.ProcessBarrierCollision();
        }
    }
}
