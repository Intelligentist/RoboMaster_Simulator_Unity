using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.Vehicles.Car;
public class EngineerLocalPlayer : NetworkBehaviour {
    public GameObject meum;
    public GameObject maincamera;
	// Use this for initialization
	void Start () {
        if (!isLocalPlayer)
        {           
            meum.SetActive(false);
            maincamera.GetComponent<Camera>().enabled = false;
            Uncontrol();
        }
	}

    public void Uncontrol()
    {
        GetComponent<EngineerCarControl>().enabled = false;
        GetComponent<EngineerCarUserControl>().enabled = false;
        GetComponent<EngineerFireLook>().enabled = false;
        GetComponent<EngineerStretch>().enabled = false;
        GetComponent<EngineerStretch1>().enabled = false;
        GetComponent<Rotate>().enabled = false;
        GetComponent<Rotate1>().enabled = false;

    }

}
