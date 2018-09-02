using GiraffeStar;
using UnityEngine;

public class EndPoint : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Race End!!");

        new PopupMsg()
        {
            Message = "Well Done!! You escaped!!",
        }.Dispatch();
    }
}
