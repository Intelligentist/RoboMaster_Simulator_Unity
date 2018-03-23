using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineerClipCollider : MonoBehaviour {
    public EngineerClip Clip;
    public Transform clips;
    private Transform Area;
	void Start () {
    Area = GameObject.Find("Area").transform;
	}

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Obstacle")
        {
            if (Clip.HasRotateFlag)
            {
                other.transform.parent.parent.parent = clips;
                other.transform.parent.parent.GetComponent<Rigidbody>().useGravity = false;
                other.transform.parent.parent.GetComponent<Rigidbody>().isKinematic = true;

            }
            if (!Clip.HasRotateFlag)
            {
                other.transform.parent.parent.parent = Area;
                other.transform.parent.parent.GetComponent<Rigidbody>().useGravity = true;
                other.transform.parent.parent.GetComponent<Rigidbody>().isKinematic = false;

            }
        }


    }
}
