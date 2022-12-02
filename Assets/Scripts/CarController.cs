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

    private bool isGrounded = false;
    private Vector3 startPos;
    public Quaternion startRotate;

    void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Floor")
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Obstacle")
        {
            carSpeed = -0.6f * carSpeed;
        }
    }


    void Start()
    {
        startPos = transform.position;
        startRotate = transform.rotation;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            transform.position = startPos;
            transform.rotation = startRotate;
            carSpeed = 0;
            return;
        }

        // Car movement
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        if (!isGrounded)
        {
            vertical = 0;
        }

        // 四个车轮绕 X 轴旋转
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

        // Cannot turn around around X axis
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, transform.eulerAngles.z);

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
            horizontal = 0;
            carSpeed = 0;
        }
        transform.Translate(0, 0, Time.deltaTime * carSpeed);
        transform.Rotate(0, horizontal * Time.deltaTime * 150, 0);
    }
}
