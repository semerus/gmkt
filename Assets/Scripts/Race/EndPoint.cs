using GiraffeStar;
using UnityEngine;

public class EndPoint : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            Debug.Log("Race End!!");

            new PopupMsg()
            {
                Message = "Well Done!! You escaped!!",
            }.Dispatch();

            new ShowCreditSequenceMsg().Dispatch();
        }
    }
}
