using UnityEngine;

public class Car : MonoBehaviour, ITimeHandler
{
    public float AccelerationFactor = 1f;
    public float RotationFactor;
    public float FrictionFactor = 1f;
    public float MaxTranslationForce;

    private InputHandler inputHandler;

    private Vector3 rawForcePerFrame;
    private Vector3 momentum;
    private float frameAccelerationForce;
    private float frameRotationForce;
    private float actualForce;
    private float lastEnergy;

    void Start()
    {
        inputHandler = gameObject.GetComponent<InputHandler>();
        TimeSystem.GetTimeSystem().AddTimer(this);

        inputHandler.OnUpKey = () => { AddForce(Vector3.back); };
        inputHandler.OnDownKey = () => { AddForce(Vector3.forward); };
        inputHandler.OnLeftKey = () => { AddForce(Vector3.left); };
        inputHandler.OnRightKey = () => { AddForce(Vector3.right); };

        inputHandler.OnInputFinish = ProcessForce;
    }

    public void RunTime()
    {
        DriveUsingTranslation();
    }

    void AddForce(Vector3 direction)
    {
        var normalized = direction;
        rawForcePerFrame += normalized;
    }

    void ProcessForce()
    {
        frameAccelerationForce = Mathf.Abs(rawForcePerFrame.z * AccelerationFactor);
        frameRotationForce = rawForcePerFrame.x * RotationFactor;

        if (lastEnergy * FrictionFactor + frameAccelerationForce > MaxTranslationForce)
        {
            Debug.Log("Reached maximum velocity");
        }
        actualForce = Mathf.Max(0f, Mathf.Min(lastEnergy * FrictionFactor + frameAccelerationForce, MaxTranslationForce));
        lastEnergy = actualForce;

        rawForcePerFrame = Vector3.zero;
    }

    void DriveUsingTranslation()
    {
        if (actualForce > 0f)
        {
            //Debug.Log("current speed" + actualForce);
        }
        transform.Rotate(new Vector3(0f, -frameRotationForce * 10f, 0f));
        transform.Translate(Vector3.back * actualForce);
    }

    public void StopCar()
    {

    }

    public void BlinkCar()
    {

    }
}
