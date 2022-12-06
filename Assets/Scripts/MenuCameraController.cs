using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCameraController : MonoBehaviour
{

    void Start()
    {

    }

    
    void Update()
    {
        transform.Rotate(new Vector3(0, Time.deltaTime * 10, - (Time.deltaTime * 10 / Mathf.Sqrt(3))));
    }
}
