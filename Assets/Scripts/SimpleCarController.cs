using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCarController : MonoBehaviour
{
    public float maxSpeed = 20f;

    private float accleration = 20f;
    private float decceleration = 15f;
    private float carSpeed = 0;

    private float horizontal;
    private float vertical;

    private bool isGrounded = false;

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


    void Start()
    {

    }

    void Update()
    {
        // Car movement
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        if (!isGrounded)
        {
            vertical = 0;
        }
    }

    void FixedUpdate()
    {

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
        }

        transform.Translate(0, 0, Time.deltaTime * carSpeed);
        transform.Rotate(0, horizontal * Time.deltaTime * 150, 0);
    }
}
