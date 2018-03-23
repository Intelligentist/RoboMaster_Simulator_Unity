using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueSupportKill : MonoBehaviour
{
    public bool BlueKillflag;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Hiter" && other.GetComponent<HitCounter>().robotcolor == Color.red)
        {
            //BlueKillflag = true;
        }
    }


}
