using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandCollider : MonoBehaviour {
    public bool collisionflag = false;
    void OnCollisionEnter(Collision other)
    {
        if (other.rigidbody.tag == "Island")
        {
            collisionflag = true;
        }
        else
        {
            collisionflag = false; 
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
