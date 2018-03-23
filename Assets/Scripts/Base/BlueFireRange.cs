using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueFireRange : MonoBehaviour
{

    public bool BaseFireFlag = false;
    public Vector3 LeftHiterPos;
    public Vector3 RightHiterPos;
    public Vector3 FrontHiterPos;
    public Vector3 BackHiterPos;
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Hiter" && other.GetComponent<HitCounter>().robotcolor == Color.red  )
        {
            if (other.GetComponent<HitCounter>().robothealth == 0)
            {
                BaseFireFlag = false;
            }
            if (other.gameObject.name == "Left" && other.GetComponent<HitCounter>().robothealth > 0)
            {
                LeftHiterPos = other.gameObject.transform.position;
                BaseFireFlag = true;
            }
            if (other.gameObject.name == "Right" && other.GetComponent<HitCounter>().robothealth > 0)
            {
                RightHiterPos = other.gameObject.transform.position;
                BaseFireFlag = true;
            }
            if (other.gameObject.name == "Front" && other.GetComponent<HitCounter>().robothealth > 0)
            {
                FrontHiterPos = other.gameObject.transform.position;
                BaseFireFlag = true;
            }
            if (other.gameObject.name == "Back" && other.GetComponent<HitCounter>().robothealth > 0)
            {
                BackHiterPos = other.gameObject.transform.position;
                BaseFireFlag = true;
            }



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
