using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedFireRange : MonoBehaviour
{

    public bool BaseFireFlag = false;
    public Vector3 LeftHiterPos;
    public Vector3 RightHiterPos;
    public Vector3 FrontHiterPos;
    public Vector3 BackHiterPos;
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Hiter" && other.GetComponent<HitCounter>().robotcolor == Color.blue && other.GetComponent<HitCounter>().robothealth >0)
        {
            if (other.gameObject.name == "Left")
            {
                LeftHiterPos = other.gameObject.transform.position;
            }
            if (other.gameObject.name == "Right")
            {
                RightHiterPos = other.gameObject.transform.position;
            }
            if (other.gameObject.name == "Front")
            {
                FrontHiterPos = other.gameObject.transform.position;
            }
            if (other.gameObject.name == "Back")
            {
                BackHiterPos = other.gameObject.transform.position;
            }
            BaseFireFlag = true;
        }


    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Hero" || other.gameObject.tag == "Infantry" || other.gameObject.tag == "CheckIn")
        {
            BaseFireFlag = false;
        }
    }
}
