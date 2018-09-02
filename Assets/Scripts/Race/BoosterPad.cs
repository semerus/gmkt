using UnityEngine;

public class BoosterPad : MonoBehaviour
{
    [HideInInspector]
    public GameObject NextGo;
    public GameObject TargetPos;
    public static bool IsOnBooster { get; private set; }
    public static BoosterPad currentPad;

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            IsOnBooster = true;
            currentPad = this;
        }

        Debug.Log("On Enter");
    }

    void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            IsOnBooster = false;
            currentPad = null;
            gameObject.SetActive(false);
        }

        Debug.Log("On Exit");
    }

    public void Reset()
    {
        IsOnBooster = false;
        currentPad = null;
        gameObject.SetActive(true);
    }
}
