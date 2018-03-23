using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.Vehicles.Car;
public class LocalPlayers : NetworkBehaviour {
    public GameObject meum;
    public GameObject hitblink;
    public GameObject maincamera;
    public GameObject camera1;
    // Use this for initialization
    void Start () {
        if (!isLocalPlayer)
        {
            Uncontrol();
            meum.SetActive(false);
            maincamera.GetComponent<Camera>().enabled = false;
            camera1.GetComponent<Camera>().enabled = false;
        }
	}

    public void Uncontrol()
    {
        GetComponent<CarController>().enabled = false;
        GetComponent<CarUserControl>().enabled = false;
        GetComponent<Fire>().enabled = false;
        GetComponent<FireLook>().enabled = false;
        GetComponent<Health>().enabled = false;
        GetComponent<Stretch1>().enabled = false;
        GetComponent<Stretch2>().enabled = false;
        hitblink.SetActive(false);
    }

}
