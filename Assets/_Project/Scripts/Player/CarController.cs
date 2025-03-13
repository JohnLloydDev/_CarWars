using Photon.Pun;
using UnityEngine;

public class CarController : MonoBehaviourPun
{
    public float speed = 1500f;
    public float turnSpeed = 100f;
    public Transform wheelFL, wheelFR, wheelBL, wheelBR;  // Assign in Inspector
    public Rigidbody rb;

    private float moveInput;
    private float turnInput;

    public float AccelInput { get; internal set; }

    void Start()
    {
        if (!photonView.IsMine)   // Disable controls for other players' cars
        {
            GetComponent<CarController>().enabled = false;
            return;
        }

        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (!photonView.IsMine) return; // Only control local player

        moveInput = Input.GetAxis("Vertical");
        turnInput = Input.GetAxis("Horizontal");

        RotateWheels();
    }

    void FixedUpdate()
    {
        if (!photonView.IsMine) return;

        MoveCar();
        TurnCar();
    }

    void MoveCar()
    {
        rb.AddForce(transform.forward * moveInput * speed * Time.fixedDeltaTime, ForceMode.Acceleration);
    }

    void TurnCar()
    {
        rb.AddTorque(transform.up * turnInput * turnSpeed * Time.fixedDeltaTime, ForceMode.Acceleration);
    }

    void RotateWheels()
    {
        float rotationSpeed = moveInput * speed * Time.deltaTime;
        wheelFL.Rotate(rotationSpeed, 0, 0);
        wheelFR.Rotate(rotationSpeed, 0, 0);
        wheelBL.Rotate(rotationSpeed, 0, 0);
        wheelBR.Rotate(rotationSpeed, 0, 0);
    }
}
