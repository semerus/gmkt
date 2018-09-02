using GiraffeStar;
using UnityEngine;

public class Car : MonoBehaviour, ITimeHandler
{
    public bool UseAddForce = false;

    // translation mode
    [Header("Translation Mode")]
    //public float AccelerationFactor = 1f;
    //public float FrictionFactor = 1f;
    //public float MaxTranslationForce;
    public float AccelerationMultiplier;
    public float RotationFactor;
    public float FrictionMultiplier;
    public float BarrierPenaltyMultiplier;
    //public float savedMagnitude;
    //public Vector3 savedDirection;

    [Header("AddForce Mode")]
    public float ForceMultiplier = 1f;
    public float MaximumVelocity = 10f;


    private InputHandler inputHandler;
    public RaceCamera playerCamera { get; private set; }
    private Rigidbody rigidbody;

    private Vector3 rawForcePerFrame;
    private Vector3 momentum;
    private float frameAccelerationForce;
    private float frameRotationForce;
    //private float actualForce;
    Vector3 lastEnergy;
    private float magnitude;

    private UIModule uiModule;

    //Audio
    private AudioSource dragging;

    void Start()
    {
        inputHandler = gameObject.GetComponent<InputHandler>();
        TimeSystem.GetTimeSystem().AddTimer(this);
        uiModule = GiraffeSystem.FindModule<UIModule>();

        playerCamera = gameObject.GetComponentInChildren<RaceCamera>();
        rigidbody = gameObject.GetComponentInChildren<Rigidbody>();

        inputHandler.OnUpKey = () => { AddForce(Vector3.back); };
        inputHandler.OnDownKey = () => { AddForce(Vector3.forward); };
        inputHandler.OnLeftKey = () => { AddForce(Vector3.left); };
        inputHandler.OnRightKey = () => { AddForce(Vector3.right); };
        inputHandler.OnInputFinish = ProcessForce;

        // audio
        dragging = gameObject.FindChildByName("Dragging").GetComponent<AudioSource>();
    }

    public void RunTime()
    {
        if(!gameObject.activeSelf) { return; }
        if (UseAddForce)
        {
            DriveUsingAddForce();
        }
        else
        {
            DriveUsingTranslation();
        }
    }

    void AddForce(Vector3 direction)
    {
        var normalized = direction;
        rawForcePerFrame += normalized;
    }

    void ProcessForce()
    {
        // translation mode
        frameRotationForce = rawForcePerFrame.x * RotationFactor;
        frameAccelerationForce = rawForcePerFrame.z * AccelerationMultiplier;
        
        //actualForce = Mathf.Max(0f, Mathf.Min(lastEnergy * FrictionFactor + frameAccelerationForce, MaxTranslationForce));
        //lastEnergy = actualForce;

        
        
        
        
        
        // momentum mode
        var normalized = rawForcePerFrame.normalized;
        magnitude = Vector3.Magnitude(rawForcePerFrame * ForceMultiplier);
        magnitude = Mathf.Min(MaximumVelocity, magnitude);
        momentum = normalized * magnitude;

        rawForcePerFrame = Vector3.zero;
    }

    void DriveUsingTranslation()
    {
        transform.Rotate(new Vector3(0f, -frameRotationForce * 10f, 0f));
        var frameForce = -transform.forward * frameAccelerationForce;
        lastEnergy = lastEnergy * FrictionMultiplier + frameForce;
        transform.Translate(lastEnergy, Space.World);

        uiModule.ShowSpeed(lastEnergy.magnitude);



        ControlDraggingSound(lastEnergy.magnitude > 0.01f);
    }

    void DriveUsingAddForce()
    {
        transform.Rotate(new Vector3(0f, -frameRotationForce * 10f, 0f));
        rigidbody.AddForce(-transform.forward * magnitude);
    }

    public void StartCar()
    {
        if (inputHandler != null)
        {
            inputHandler.enabled = true;
        }

        if (playerCamera == null)
        {
            playerCamera = gameObject.GetComponentInChildren<RaceCamera>();
        }

        playerCamera.ChangeToPlayingMode();
    }

    public void StopCar()
    {
        if (inputHandler != null)
        {
            inputHandler.enabled = false;
        }
        rawForcePerFrame = Vector3.zero;
        lastEnergy = Vector3.zero;
        ControlDraggingSound(false);
    }

    public void BlinkCar()
    {

    }

    public void ProcessBarrierCollision()
    {
        lastEnergy = lastEnergy * BarrierPenaltyMultiplier;
        //Debug.Log("Safe barrier activated");
    }

    public void ControlDraggingSound(bool play)
    {
        if (play)
        {
            if (dragging.isPlaying)
            {
                return;
            }
            dragging.Play();
        }
        else
        {
            dragging.Stop();
        }
        
    }
}
