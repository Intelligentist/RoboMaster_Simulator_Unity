using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineerUpFork : MonoBehaviour
{
    private Transform Area;
    public Transform Fork;
    public Rotate1 rotate;
    // Use this for initialization
    void Start()
    {
        Area = GameObject.Find("Area").transform;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Obstacle")
        {
            if (!rotate.HasRotateFlag)
            {
                other.transform.parent.parent = Fork;
                other.transform.parent.GetComponent<Rigidbody>().useGravity = false;
                other.transform.parent.GetComponent<Rigidbody>().isKinematic = true;
                other.transform.parent.transform.Find("collision").gameObject.SetActive(false);
            }
            else
            {
                other.transform.parent.parent = Area;
                other.transform.parent.GetComponent<Rigidbody>().useGravity = true;
                other.transform.parent.GetComponent<Rigidbody>().isKinematic = false;
                other.transform.parent.transform.Find("collision").gameObject.SetActive(true);
            }
        }


    }
}
