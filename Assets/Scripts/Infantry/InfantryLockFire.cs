using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfantryLockFire : MonoBehaviour {
    public bool lockfireflag = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    void OnCollisionStay(Collision other)
    {
        if (other.collider.tag == "convex")
        {
            lockfireflag = true;
        }

    }
    void OnCollisionExit(Collision other)
    {
        lockfireflag = false;
    }

}
