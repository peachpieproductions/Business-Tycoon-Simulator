using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[System.Serializable]
public class Vehicle_Wheels : System.Object
{
    public WheelCollider leftWheel;
    public GameObject leftWheelMesh;
    public WheelCollider rightWheel;
    public GameObject rightWheelMesh;
    public bool motor;
    public bool steering;
    public bool reverseTurn;
}

public class VehicleController : MonoBehaviour
{
    [Header("Vehicle Settings")]
    public float maxMotorTorque;
    public float maxSteeringAngle;

    [Header("Camera Objects")]
    public GameObject ThirdPersonCam;
    public GameObject FirstPersonCam;
    
    [Space]

    public bool InVehicle = false;

    [Header("Camera View")]
    public int View = 0;

    [Header("Forward or Reverse")]
    public int Direction = 0;

    [Header("Other")]
    public Text SpeedText;
    public GameObject Exhaust;

    [Header("Center Of Mass")]
    public Vector3 CenterOfMass;

    [Header("Wheels")]
    public List<Vehicle_Wheels> Vehicle_Info;

    private float motor;
    private float steering;
    private float brakeTorque;

    private float TimeT;    

    public void VisualizeWheel(Vehicle_Wheels wheelPair)
    {
        Quaternion rot;
        Vector3 pos;
        wheelPair.leftWheel.GetWorldPose(out pos, out rot);
        wheelPair.leftWheelMesh.transform.position = pos;
        wheelPair.leftWheelMesh.transform.rotation = rot;
        wheelPair.rightWheel.GetWorldPose(out pos, out rot);
        wheelPair.rightWheelMesh.transform.position = pos;
        wheelPair.rightWheelMesh.transform.rotation = rot;
    }
    
    void Update()
    {
        Vehicle();

        GetComponent<Rigidbody>().centerOfMass = CenterOfMass;
        
        //Check if the player is in this vehicle. If so allow the player to control it
        if (InVehicle == true)
        {
            Cursor.visible = false;

            //Set Camera View to 3rd or 1st
            if (View == 0)
            {
                FirstPersonCam.SetActive(true);
                ThirdPersonCam.SetActive(false);
            }
            if (View == 1)
            {
                ThirdPersonCam.SetActive(true);
                FirstPersonCam.SetActive(false);                
            }

            //Call controls update
            VehicleControl();

            GetComponent<AudioSource>().enabled = true;
            if(Exhaust!=null)
                Exhaust.SetActive(true);

            //Update text with the vehicles speed
            SpeedText.text = (GetComponent<Rigidbody>().velocity.magnitude * 3.6f ).ToString("f0") + " km";
        }
        else
        {
            FirstPersonCam.SetActive(false);
            ThirdPersonCam.SetActive(false);
            GetComponent<AudioSource>().enabled = false;
            if (Exhaust != null)
                Exhaust.SetActive(false);
        }

        //Adjust pitch of engine sound to sound like acceleration
        float minPitch = 0.9f;
        float maxPitch = 1.7f;

        float pitchModifier = maxPitch - minPitch;

        GetComponent<AudioSource>().pitch = minPitch + (GetComponent<Rigidbody>().velocity.magnitude/15) * pitchModifier;
        
        //Change Camera View to 3rd or 1st
        TimeT += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.V) && View == 0 && TimeT > 0.2f)
        {
            View = 1;
            TimeT = 0;
        }
        if (Input.GetKeyDown(KeyCode.V) && View == 1 && TimeT > 0.2f)
        {
            View = 0;
            TimeT = 0;
        }

    }
    
    //Send controls(Only called when in this vehicle)
    void VehicleControl()
    {        
        motor = maxMotorTorque * Input.GetAxis("Vertical");
        steering = maxSteeringAngle * Input.GetAxis("Horizontal");
        brakeTorque = Mathf.Abs(Input.GetAxis("Jump"));
    }

    //Main vehicle controls
    void Vehicle()
    {
        //Come to a complete stop
        if (GetComponent<Rigidbody>().velocity.magnitude < 1 && ((!Input.GetKey(KeyCode.W) && Direction == 0) || (!Input.GetKey(KeyCode.S) && Direction == 1)))
        {
            motor = 0;
        }

        //Change vehicle direction
        if (Input.GetKey(KeyCode.W) && Direction == 1 && GetComponent<Rigidbody>().velocity.magnitude < 0.3)
            Direction = 0;

        if (Input.GetKey(KeyCode.S) && Direction == 0 && GetComponent<Rigidbody>().velocity.magnitude < 0.3)
            Direction = 1;

        //Break
        if (brakeTorque > 0.001 || (!Input.GetKey(KeyCode.W) && Direction == 0) || (!Input.GetKey(KeyCode.S) && Direction == 1))
        {
            brakeTorque = maxMotorTorque;
            motor = 0;
        }
        else
        {
            brakeTorque = 0;
        }

        //Update wheels with steering and torque information
        foreach (Vehicle_Wheels Vehicle_Infos in Vehicle_Info)
        {
            if (Vehicle_Infos.steering == true)
            {
                Vehicle_Infos.leftWheel.steerAngle = Vehicle_Infos.rightWheel.steerAngle = ((Vehicle_Infos.reverseTurn) ? -1 : 1) * steering;
            }

            if (Vehicle_Infos.motor == true)
            {
                Vehicle_Infos.leftWheel.motorTorque = motor;
                Vehicle_Infos.rightWheel.motorTorque = motor;
            }

            Vehicle_Infos.leftWheel.brakeTorque = brakeTorque;
            Vehicle_Infos.rightWheel.brakeTorque = brakeTorque;

            VisualizeWheel(Vehicle_Infos);

        }
    }
}
