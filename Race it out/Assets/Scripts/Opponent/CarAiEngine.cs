using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAiEngine : MonoBehaviour
{
    Rigidbody rigidbody;
    public Transform path;
    [SerializeField] Vector3 centreOfMass;

    [SerializeField] WheelCollider frontLeftWheelCollider;
    [SerializeField] WheelCollider frontRightWheelCollider;
    [SerializeField] WheelCollider rearLeftWheelCollider;
    [SerializeField] WheelCollider rearRightWheelCollider;
    
    [SerializeField] float maxMotorTorque = 2000f;
    [SerializeField] float currentSpeed;
    [SerializeField] float maxSpeed = 3000f;
    [SerializeField] float maxBrakeTorque = 3000f;
    [SerializeField] float maxSteerAngle = 45f;
    [SerializeField] bool isBraking = false;

    [Header("Sensors")]
    [SerializeField] float sensorLength = 1f;
    [SerializeField] Vector3 frontSensorPos =new Vector3(0f, 0.2f,0.5f);
    [SerializeField] float frontSideSensorPos = 0.5f;
    [SerializeField] float frontSensorAngle = 30f;

    bool isavoiding = false;
    List<Transform> nodes;
    int currentNode = 0;
    bool canRespwan = true;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.centerOfMass = centreOfMass;
	    Transform[] pathTransforms = path.GetComponentsInChildren<Transform>();
        nodes = new List<Transform>();
        
	    for(int i = 0; i < pathTransforms.Length;i++)
	    {
		    if(pathTransforms[i] != path.transform)
		    {
			    nodes.Add(pathTransforms[i]);
		    }
	    }
    }

    private void FixedUpdate()
    {
        ///Sensors();
        ApplySteer();
        Drive();
        CheckWaypointDistance();
        Braking();
        StartCoroutine("Respawn");
    }

    private void Sensors()
    {
        RaycastHit hit;
        Vector3 sensorStartPos = transform.position;
        sensorStartPos += transform.forward * frontSensorPos.z;
        sensorStartPos += transform.up * frontSensorPos.y;
        isavoiding = false;
        float avoidMultiplier = 0f;
        float avoidmultiplierValue = 0.5f;

        //front right sesnor
        sensorStartPos += transform.right * frontSideSensorPos;
        if(Physics.Raycast(sensorStartPos,transform.forward,out hit,sensorLength))
        {
            if (hit.collider.CompareTag("Untagged"))
            {
                Debug.DrawLine(sensorStartPos, hit.point);
                isavoiding = true;
                avoidMultiplier -= avoidmultiplierValue;
            }
        }

        //front right angle sesnor
        if (Physics.Raycast(sensorStartPos,Quaternion.AngleAxis(frontSensorAngle,transform.up)* transform.forward, out hit, sensorLength)) //Multiply by transform.up to get a vector 3
        {
            if (hit.collider.CompareTag("Untagged"))
            {
                Debug.DrawLine(sensorStartPos, hit.point);
                isavoiding = true;
                avoidMultiplier -= avoidmultiplierValue/2f;
            }
        }

        //front left sesnor
        sensorStartPos -= transform.right * frontSideSensorPos * 2;
        if (Physics.Raycast(sensorStartPos,transform.forward,out hit,sensorLength))
        {
            if (hit.collider.CompareTag("Untagged"))
            {
                Debug.DrawLine(sensorStartPos, hit.point);
                isavoiding = true;
                avoidMultiplier += avoidmultiplierValue;
            }
        }

        //front left angle sesnor
         if (Physics.Raycast(sensorStartPos, Quaternion.AngleAxis(-frontSensorAngle, transform.up) * transform.forward, out hit, sensorLength)) //Multiply by transform.up to get a vector 3
        {
            if (hit.collider.CompareTag("Untagged"))
            {
                Debug.DrawLine(sensorStartPos, hit.point);
                isavoiding = true;
                avoidMultiplier += avoidmultiplierValue/2f;
            }
        }

        //front sensor
        //sensorStartPos += transform.forward * frontSensorPos.z;
        if (avoidMultiplier == 0)
        {
            if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength))
            {
                if (hit.collider.CompareTag("Untagged"))
                {
                     Debug.DrawLine(sensorStartPos, hit.point);
                    isavoiding = true;
                    if(hit.normal.x < 0)
                    {
                        avoidMultiplier = -avoidmultiplierValue;
                    }
                    else
                    {
                        avoidMultiplier = avoidmultiplierValue;
                    }
                }
            }
        }

        if (isavoiding)
        {
            frontLeftWheelCollider.steerAngle = maxSteerAngle * avoidMultiplier;
            frontRightWheelCollider.steerAngle = maxSteerAngle * avoidMultiplier;

        }
    }

    private void ApplySteer()
    {
        if (isavoiding) { return; }
        Vector3 relativeVector = transform.InverseTransformPoint(nodes[currentNode].position);
        float newSteer = (relativeVector.x / relativeVector.magnitude)*maxSteerAngle;
        frontLeftWheelCollider.steerAngle = newSteer;
        frontRightWheelCollider.steerAngle = newSteer;
    }

    private void Drive()
    {
        currentSpeed = rigidbody.velocity.magnitude * 3.6f;
        if(currentSpeed < maxSpeed && !isBraking)
        {
            frontLeftWheelCollider.motorTorque = maxMotorTorque;
            frontRightWheelCollider.motorTorque = maxMotorTorque;
        }
        else
        {
            frontLeftWheelCollider.motorTorque = 0;
            frontRightWheelCollider.motorTorque = 0;
        }
    }

    private void CheckWaypointDistance()
    {
        if (Vector3.Distance(transform.position, nodes[currentNode].position) < 5f)
        {
            if(currentNode == nodes.Count-1)
            {
                currentNode = 0;
            }
            else
            {
                currentNode++;
            }
        }
    }

    private void Braking()
    {
        if(isBraking)
        {
            rearLeftWheelCollider.brakeTorque = maxBrakeTorque;
            rearRightWheelCollider.brakeTorque = maxBrakeTorque;
        }
        else
        {
            rearLeftWheelCollider.brakeTorque = 0;
            rearRightWheelCollider.brakeTorque = 0f;
        }
    }

    private IEnumerator Respawn()
    {
        float currentPos = transform.position.z;
        yield return new WaitForSeconds(5f);
        float newPos = transform.position.z;
        if(Mathf.Abs(currentPos-newPos) < 0.1f && canRespwan)
        {
            transform.Rotate(new Vector3(0, 90f, 0f));
            canRespwan = false;
            StartCoroutine("RespawnResetter");
        }
    }

    private IEnumerator RespawnResetter()
    {
        yield return new WaitForSeconds(5f);
        canRespwan = true;
    }

}
