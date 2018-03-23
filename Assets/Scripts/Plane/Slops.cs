using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;

public class Slops : MonoBehaviour {
    private MeshCollider slops;
    
	// Use this for initialization
    void Awake()
    {
        slops = GetComponent<MeshCollider>();

    }

    void OnCollisionEnter(Collision other)
    {


        if (other.rigidbody.tag == "Infantry" || other.rigidbody.tag == "Engineer")
        {
                slops.isTrigger = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        slops.isTrigger = false;
    }


}
