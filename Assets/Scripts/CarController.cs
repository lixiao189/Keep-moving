using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public float maxSpeed = 20f;

    private float accleration = 20f;
    private float decceleration = 15f;
    private float carSpeed = 0;

    private float horizontal;
    private float vertical;

    private Rigidbody carRigidbody;
    private bool isGrounded = false;
    private Vector3 startPos;
    private Quaternion startRotate;

    void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Floor")
        {
            isGrounded = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Floor")
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
        if (Input.GetKey(KeyCode.R))
        {
            transform.position = startPos;
            transform.rotation = startRotate;

            carRigidbody.velocity = Vector3.zero;
            carSpeed = 0;

            isGrounded = false;

            return;
        }

        // Car movement
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        if (!isGrounded)
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
            if (carSpeed + accleration * Time.deltaTime <= maxSpeed)
            {
                carSpeed += accleration * Time.deltaTime;
            }
        }
        else if (carSpeed > 0)
        {
            carSpeed -= decceleration * Time.deltaTime;
        }

        if (carSpeed <= 0)
        {
            carSpeed = 0;
        }

        transform.Rotate(0, horizontal * Time.deltaTime * 150, 0);
        Debug.Log(isGrounded); // debug 
        if (isGrounded)
        {
            carRigidbody.velocity = transform.forward * carSpeed;
        }
    }
}
