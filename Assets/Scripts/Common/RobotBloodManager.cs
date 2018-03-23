using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RobotBloodManager : NetworkBehaviour
{
    [SyncVar]
    public float robotblood1;
    [SyncVar]
    public float robotblood2;
    private GameObject Infantry;
	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        Infantry = GameObject.Find("Infantry(Clone)");
        if (Infantry.GetComponent<RobotInit>().robotname == "INFANTRY1")
        {
            robotblood1 = Infantry.GetComponent<InfantryHealth>().CurrentHealth;
        }
        if (Infantry.GetComponent<RobotInit>().robotname == "INFANTRY2")
        {
            robotblood2 = Infantry.GetComponent<InfantryHealth>().CurrentHealth;
        }
    }
}
