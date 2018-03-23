using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.Vehicles.Car;
public class InfantryLocalPlayer : NetworkBehaviour {
    public GameObject meum;
    public GameObject hitblink;
    public GameObject maincamera;
	// Use this for initialization
	void Start () {
        if (!isLocalPlayer)
        {
            Uncontrol();
            meum.SetActive(false);
            maincamera.GetComponent<Camera>().enabled = false;
        }
	}

    public void Uncontrol()
    {
        GetComponent<InfantryCarControl>().enabled = false;
        GetComponent<InfantryCarUserControl>().enabled = false;
        GetComponent<InfantryFire>().enabled = false;
        GetComponent<InfantryFireLook>().enabled = false;
        GetComponent<InfantryHealth>().enabled = false;
        hitblink.SetActive(false);
    }

    }
