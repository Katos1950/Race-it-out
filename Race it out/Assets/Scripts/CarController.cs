using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    //Lambo wheels mesh and collider position was lowered to increase ground clearance
    float horizontalInput;
    float verticalInput;
    float currentBrakeForce;
    float currentSteerAngle;
    float brakeInput;
    bool isBraking;
    bool slowDownCar;
    Rigidbody rigidbody;

    [SerializeField] Vector3 centreOfMass;

    [SerializeField] float motorForce;
    [SerializeField] float brakeForce;
    [SerializeField] float slowDownForce;
    [SerializeField] float maxSteerAngle;

    [SerializeField] WheelCollider frontLeftWheelCollider;
    [SerializeField] WheelCollider frontRightWheelCollider;
    [SerializeField] WheelCollider rearLeftWheelCollider;
    [SerializeField] WheelCollider rearRightWheelCollider;

    [SerializeField] Transform frontLeftWheelTransform;
    [SerializeField] Transform frontRightWheelTransform;
    [SerializeField] Transform rearLeftWheelTransform;
    [SerializeField] Transform rearRightWheelTransform;

    [Header("Audio")]
    [SerializeField] AudioClip engineSFX;
    [SerializeField] float startPitch;
    [SerializeField] float endPitch;
    [SerializeField] AudioClip brakeSFX;
    [SerializeField] AudioSource audioSource1;
    [SerializeField] AudioSource audioSource2;
    float pitchRange;

    public float speed;
    public float topSpeed;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.centerOfMass = centreOfMass;
        audioSource1 = GetComponent<AudioSource>();
        pitchRange = endPitch - startPitch; 
    }

    private void FixedUpdate()
    {
        GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheels();
    }

    private void GetInput()
    {
        horizontalInput = SimpleInput.GetAxis("Horizontal");
        verticalInput = SimpleInput.GetAxis("Vertical");
        brakeInput = SimpleInput.GetAxis("Jump");
        isBraking = brakeInput > 0.1 ? true : false;
    }

    private void HandleMotor()
    {
        speed = Mathf.Abs(rigidbody.velocity.magnitude) * 3.6f;//abs for clamping reverse speed also , *3.6 to covert it to kmph

        HandleEngineAudio();
        //added later
        if (speed > topSpeed)
        {
            slowDownCar = true;
        }
        if (speed < topSpeed && verticalInput != 0)
        {
            rearLeftWheelCollider.motorTorque = verticalInput * motorForce;
            rearRightWheelCollider.motorTorque = verticalInput * motorForce;
            slowDownCar = false;
        }
        if (verticalInput == 0)
        {
            slowDownCar = true;
            rearLeftWheelCollider.motorTorque = 0;
            rearRightWheelCollider.motorTorque = 0;

        }
        ApplyBrakes();
    }

    private void HandleEngineAudio()
    {
        audioSource1.clip = engineSFX;
        float pitchCal = (speed / topSpeed) * pitchRange + startPitch;
        audioSource1.pitch = Mathf.Clamp(pitchCal, startPitch, endPitch);

        if (!audioSource1.isPlaying)
        {
            audioSource1.Play();
        }
    }

    private void ApplyBrakes()
    {
        if(slowDownCar)
        {
            if(speed > topSpeed)
            {
                currentBrakeForce = motorForce;
            }
            else
            {
                currentBrakeForce = slowDownForce;
            }
        }
        else
        {
            currentBrakeForce = isBraking ? brakeForce : 0;
        }
        HandleBrakeAudio();
        //uncommented the frontwheelcollider
        frontLeftWheelCollider.brakeTorque = currentBrakeForce;
        frontRightWheelCollider.brakeTorque = currentBrakeForce;
        rearLeftWheelCollider.brakeTorque = currentBrakeForce;
        rearRightWheelCollider.brakeTorque = currentBrakeForce;
    }

    private void HandleBrakeAudio()
    {
        if (isBraking && speed > 1)
        {
            audioSource2.clip = brakeSFX;

            if (!audioSource2.isPlaying)
            {
                audioSource2.Play();
            }
        }
        else
        {
            audioSource2.Stop();
        }
    }

    private void HandleSteering()
    {
        currentSteerAngle = horizontalInput * maxSteerAngle;
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(frontLeftWheelCollider,frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider,frontRightWheelTransform);
        UpdateSingleWheel(rearLeftWheelCollider,rearLeftWheelTransform);
        UpdateSingleWheel(rearRightWheelCollider,rearRightWheelTransform);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.position = pos;
        wheelTransform.rotation = rot;
    }
}

/* old
 * if(speed < topSpeed && verticalInput != 0)
        {
            Debug.Log(verticalInput);
            //frontLeftWheelCollider.motorTorque = verticalInput * motorForce;
            //frontRightWheelCollider.motorTorque = verticalInput * motorForce;
            rearLeftWheelCollider.motorTorque = verticalInput * motorForce;
            rearRightWheelCollider.motorTorque = verticalInput * motorForce;
        }
        else if(verticalInput !< 0 && !isBraking || verticalInput == 0 || speed > topSpeed)
        {
            frontLeftWheelCollider.motorTorque = 0 ;
            frontRightWheelCollider.motorTorque = 0;
            rearLeftWheelCollider.motorTorque = 0;
            rearRightWheelCollider.motorTorque = 0;
        }*/