using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestinationController : MonoBehaviour
{
    public GameObject hydrant1;
    public GameObject hydrant2;

    private ParticleSystem ps1;
    private ParticleSystem ps2;

    void OnCollisionStay(Collision other)
    {
        if (other.transform.tag == "Player" && other.rigidbody.velocity.magnitude <= 0.1f)
        {
            ps1.Play();
            ps2.Play();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        ps1 = hydrant1.GetComponent<ParticleSystem>();
        ps2 = hydrant2.GetComponent<ParticleSystem>();

        ps1.Stop();
        ps2.Stop();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
