using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public float maxSpeed = 20f;

    // Stop sign
    public GameObject stopSignPrefeb;
    private GameObject stopSign;

    private float accleration = 20f;
    private float decceleration = 15f;
    private float carSpeed = 0;

    private float horizontal;
    private float vertical;

    private Rigidbody carRigidbody;
    private bool isGrounded = false;
    private Vector3 startPos;
    private Quaternion startRotate;

    // Game state
    private bool isStarted = false;
    private bool isSuccessful = false;
    private bool isEnded = false;
    private int gameTimes = 3;

    void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Floor" || other.transform.tag == "Destination")
        {
            isGrounded = true;
        }

        if (other.transform.tag == "Destination" && carSpeed <= 0)
        {
            Debug.Log("Success!");
            isEnded = true;
            isSuccessful = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        isGrounded = false;
    }

    void OnCollisionStay(Collision other)
    {
        if (other.transform.tag == "Water")
        {
            isGrounded = false;
        }
    }

    void Start()
    {
        startPos = transform.position;
        startRotate = transform.rotation;
        carRigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Debug.Log(gameTimes); // debug
        if (Input.GetKeyDown(KeyCode.R) && gameTimes > 0) // Reset 
        {
            transform.position = startPos;
            transform.rotation = startRotate;

            carRigidbody.velocity = Vector3.zero;
            carSpeed = 0;

            isGrounded = false;

            // Reset game state
            isStarted = false;
            isSuccessful = false;
            isEnded = false;

            // Destory stop sign
            Destroy(stopSign);

            gameTimes--;

            return;
        }

        // Car movement
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        if (!isGrounded || isEnded)
        {
            vertical = 0;
        }

        // Four wheel turn aournd the x axis
        transform.GetChild(1).Rotate(carSpeed * 100, 0, 0);
        transform.GetChild(2).Rotate(carSpeed * 100, 0, 0);
        transform.GetChild(3).Rotate(carSpeed * 100, 0, 0);
        transform.GetChild(4).Rotate(carSpeed * 100, 0, 0);
    }

    void FixedUpdate()
    {
        // Add gravity
        float mass = GetComponent<Rigidbody>().mass;
        GetComponent<Rigidbody>().AddForce(Vector3.down * mass);

        // Cannot turn around around X and Z axis
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);

        if (vertical > 0)
        {
            isStarted = true; // Game started

            if (carSpeed + accleration * Time.deltaTime <= maxSpeed)
            {
                carSpeed += accleration * Time.deltaTime;
            }
        }
        else if (carSpeed > 0)
        {
            carSpeed -= decceleration * Time.deltaTime; // Deccelerate

            // Add drag force
            if (vertical < 0)
            {
                carSpeed -= 2 * decceleration * Time.deltaTime;
            }
        }

        if (carSpeed <= 0)
        {
            carSpeed = 0;

            if (isStarted && !isEnded && isGrounded) // Game over
            {
                stopSign = Instantiate(stopSignPrefeb, transform.position + transform.forward * 4, stopSignPrefeb.transform.rotation);
                isEnded = true;

                if (!isSuccessful) // Game failed
                {
                    Debug.Log("Failed!");
                }
            }
        }

        transform.Rotate(0, horizontal * Time.deltaTime * 150, 0);

        if (isGrounded)
        {
            carRigidbody.velocity = transform.forward * carSpeed;
        }
    }
}
