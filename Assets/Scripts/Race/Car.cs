using UnityEngine;

public class Car : MonoBehaviour, ITimeHandler
{
    public float AccelerationFactor = 1f;
    public float RotationFactor;

    private InputHandler inputHandler;

    private Vector3 rawForcePerFrame;
    private Vector3 momentum;
    private float accelerationForce;
    private float rotationForce;

    void Start()
    {
        inputHandler = gameObject.GetComponent<InputHandler>();
        TimeSystem.GetTimeSystem().AddTimer(this);

        inputHandler.OnUpKey = () => { AddForce(Vector3.forward); };
        inputHandler.OnDownKey = () => { AddForce(Vector3.back); };
        inputHandler.OnLeftKey = () => { AddForce(Vector3.left); };
        inputHandler.OnRightKey = () => { AddForce(Vector3.right); };

        inputHandler.OnInputFinish = ProcessForce;
    }

    public void RunTime()
    {
        Drive();
    }

    void AddForce(Vector3 direction)
    {
        var normalized = direction;
        rawForcePerFrame += normalized;
    }

    void ProcessForce()
    {
        accelerationForce = rawForcePerFrame.z * AccelerationFactor;
        rotationForce = rawForcePerFrame.x * RotationFactor;

        rawForcePerFrame = Vector3.zero;
    }

    void Drive()
    {
        transform.Rotate(new Vector3(0f, rotationForce * 10f, 0f));
        transform.Translate(Vector3.back * accelerationForce);
    }

    public void StopCar()
    {

    }

    public void BlinkCar()
    {

    }
}
