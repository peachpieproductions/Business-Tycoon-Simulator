using UnityEngine;
//using UnityStandardAssets.Characters.FirstPerson;

public class PlayerLook : MonoBehaviour
{
    [Header("Player Camera")]
    public GameObject Cam;

    [Header("Player Object")]
    public GameObject PlayerController;

    [Header("player UI")]
    public GameObject PlayerVehicleUI;

    private GameObject CurrentVehicle;

    private RaycastHit hit;
    private float TimeT;

    //Turn off UI on start
    void Start()
    {
        PlayerVehicleUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //Timer for click delay
        TimeT += Time.deltaTime;

        //Raycast to check for vehicles
        if (Physics.Raycast(Cam.transform.position, Cam.transform.forward, out hit, 3))
        {
            //Debug.DrawLine(Cam.transform.position, hit.point, Color.green, 0);
            //Debug.Log(hit.collider.tag);
            if (Input.GetKeyDown(KeyCode.E) && hit.collider.tag == ("Vehicle") && TimeT > 0.2f && hit.transform.GetComponent<VehicleController>().InVehicle == false)
            {
                hit.transform.GetComponent<VehicleController>().InVehicle = true;
                Cam.GetComponent<Camera>().enabled = false;
                Cam.GetComponent<AudioListener>().enabled = false;
                //PlayerController.GetComponent<FirstPersonController>().enabled = false;
                PlayerController.GetComponent<CharacterController>().enabled = false;
                PlayerVehicleUI.SetActive(true);
                hit.transform.GetComponent<VehicleController>().FirstPersonCam.SetActive(true);
                
                PlayerController.transform.parent = hit.transform;

                CurrentVehicle = hit.transform.gameObject;

                //Reset click timer
                TimeT = 0;
            }
        }

        //If we dont have a current vehicle we dont need to continue so we return
        if (CurrentVehicle == null)
            return;

        //Exit the vehicle
        if (Input.GetKeyDown(KeyCode.E) && CurrentVehicle.transform.GetComponent<VehicleController>().InVehicle == true && TimeT > 0.2f)
        {
            CurrentVehicle.transform.GetComponent<VehicleController>().InVehicle = false;
            Cam.GetComponent<Camera>().enabled = true;
            Cam.GetComponent<AudioListener>().enabled = true;
            //PlayerController.GetComponent<FirstPersonController>().enabled = true;
            PlayerController.GetComponent<CharacterController>().enabled = true;
            PlayerVehicleUI.SetActive(false);
            CurrentVehicle.transform.GetComponent<VehicleController>().FirstPersonCam.SetActive(false);
            CurrentVehicle.transform.GetComponent<VehicleController>().ThirdPersonCam.SetActive(false);            

            PlayerController.transform.parent = null;

            CurrentVehicle = null;

            TimeT = 0;
        }
    }
}
