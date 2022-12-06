using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NPCCarController : MonoBehaviour
{
    public enum NPCCarState
    {
        Move,
        Stop,
        End
    }

    private Rigidbody carRigidbody;

    public NPCCarState state = NPCCarState.Stop;
    public float speed = 10f;

    public int maxTick = 200;
    private int tick = 0;

    void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Floor" || other.transform.tag == "Destination")
        {
            state = NPCCarState.Move;
        }
    }

    void OnCollisionEnter(Collision other)
    {
        Debug.Log("test " + other.transform.tag);
        if (other.transform.tag != "Floor" && other.transform.tag != "Destination")
        {
            state = NPCCarState.End;
        }
    }

    void OnTriggerExit(Collider other)
    {

    }

    void Start()
    {
        carRigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
    }

    void FixedUpdate()
    {
        tick++;
        if (tick == maxTick)
        {
            state = NPCCarState.End;
        }

        switch (state)
        {
            case NPCCarState.Move:
                carRigidbody.velocity = transform.forward * speed;
                break;
            case NPCCarState.End:
                Destroy(gameObject);
                break;
            default:
                break;
        }
    }
}
