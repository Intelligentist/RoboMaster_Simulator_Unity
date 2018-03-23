using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfantryCheckIn : MonoBehaviour {

    // Use this for initialization
    public bool checkinflag = false;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerStay(Collider other)
    {

        if (other.gameObject.tag == "CheckIn")
        {
            checkinflag = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "CheckIn")
        {
            checkinflag = false;
        }
    }


}

