using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Lean;
using Lean.Gui;
using UnityEngine.SceneManagement;

public class CarController : MonoBehaviour
{
    public float maxSpeed = 20f;

    // Stop sign
    public GameObject stopSignPrefeb;
    private GameObject stopSign;

    // Car control
    private float accleration = 20f;
    private float decceleration = 15f;
    private float carSpeed = 0;

    private float horizontal;
    private float vertical;

    private Rigidbody carRigidbody;
    private bool isGrounded = false;
    private Vector3 startPos;
    private Quaternion startRotate;
    
    private GameController gameController;
    
    void OnTriggerStay(Collider other)
    {
        if (carSpeed <= 0 && carSpeed >= -0.5)
        {
            if (other.transform.tag == "Destination")
            {
                gameController.CarParked();
            }
            else if (isGrounded 
                && gameController.state == GameController.GameState.Started
                && gameController.state != GameController.GameState.CarStopped
                && gameController.state != GameController.GameState.BeforeMove
                && gameController.state != GameController.GameState.BeforeStart)
            {
                stopSign = Instantiate(stopSignPrefeb, transform.position + transform.forward * 4, stopSignPrefeb.transform.rotation);
                gameController.CarStopped();
            }
            else
            {
                gameController.isCarInParkArea = false;
            }
        }
        if (other.transform.tag == "Floor" || other.transform.tag == "Destination")
        {
            isGrounded = true;
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
            gameController.CarStopped();
        }
    }

    void OnCollisionExit(Collision other)
    {
        if (other.transform.tag == "Obstacle")
        {
            carSpeed = -carSpeed;
        }
    }

    public void ResetPosition()
    {
        transform.position = startPos;
        transform.rotation = startRotate;

        carRigidbody.velocity = Vector3.zero;
        carSpeed = 0;

        isGrounded = false;
        Destroy(stopSign);
    }

    void Start()
    {
        startPos = transform.position;
        startRotate = transform.rotation;
        carRigidbody = GetComponent<Rigidbody>();
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
    }

    void Update()
    {
        // Car movement
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        if (gameController.state == GameController.GameState.BeforeStart && vertical > 0 && isGrounded)
        {
            gameController.StartGame();
        }
        if (gameController.state == GameController.GameState.BeforeMove && vertical > 0 && isGrounded)
        {
            gameController.CarStartMove();
        }

        if (!isGrounded || gameController.state != GameController.GameState.Started)
        {
            vertical = 0;
        }

        // Four wheel turn aournd the x axis
        transform.GetChild(1).Rotate(carSpeed * 100, 0, 0);
        transform.GetChild(2).Rotate(carSpeed * 100, 0, 0);
        transform.GetChild(3).Rotate(carSpeed * 100, 0, 0);
        transform.GetChild(4).Rotate(carSpeed * 100, 0, 0);

        Debug.Log(gameController.state);
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
            carSpeed -= decceleration * Time.deltaTime; // Deccelerate

            // Add drag force
            if (vertical < 0)
            {
                carSpeed -= 2 * decceleration * Time.deltaTime;
            }
        }

        transform.Rotate(0, horizontal * Time.deltaTime * 150, 0);
        if (isGrounded)
        {
            carRigidbody.velocity = transform.forward * carSpeed;
        }
    }
}
