using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class AxleInfo
{
    public GameObject visualLeftWheel;
    public GameObject visualRightWheel;

    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor;
    public bool steering;
}

public class CarController : MonoBehaviour
{
    public List<AxleInfo> axleInfos;
    public float maxAcceleration;
    public float brakeAcceleration;

    public float maxSteeringAngle;
    public float turnSensitivity;

    private Rigidbody rigidBody;
    public Vector3 centerOfMass;

    private float motor;
    private float steering;


    // 查找相应的可视车轮
    // 正确应用变换
    public void ApplyLocalPositionToVisuals(Transform visualWheel, WheelCollider collider)
    {
        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);

        visualWheel.transform.position = position;
        visualWheel.transform.rotation = rotation;
    }

    public void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.centerOfMass = centerOfMass;
    }

    public void Update()
    {
        motor = Input.GetAxis("Vertical");
        steering = Input.GetAxis("Horizontal");
    }

    public void LateUpdate()
    {
        foreach (AxleInfo axleInfo in axleInfos)
        {
            if (axleInfo.steering)
            {
                var steerAngle = steering * maxSteeringAngle * turnSensitivity;
                axleInfo.leftWheel.steerAngle = Mathf.Lerp(axleInfo.leftWheel.steerAngle, steerAngle, 0.6f); ;
                axleInfo.rightWheel.steerAngle = Mathf.Lerp(axleInfo.rightWheel.steerAngle, steerAngle, 0.6f); ;
            }
            if (axleInfo.motor)
            {
                // 车辆移动
                axleInfo.leftWheel.motorTorque = maxAcceleration * Time.deltaTime * motor * 600;
                axleInfo.rightWheel.motorTorque = maxAcceleration * Time.deltaTime * motor * 600;
            }
            ApplyLocalPositionToVisuals(axleInfo.visualLeftWheel.transform, axleInfo.leftWheel);
            ApplyLocalPositionToVisuals(axleInfo.visualRightWheel.transform, axleInfo.rightWheel);
        }
    }
}
