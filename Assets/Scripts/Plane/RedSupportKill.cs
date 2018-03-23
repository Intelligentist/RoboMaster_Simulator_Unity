using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedSupportKill : MonoBehaviour
{
    public bool RedKillflag = false;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Hiter" && other.GetComponent<HitCounter>().robotcolor == Color.blue)
        {
            //RedKillflag = true;
        }
    }


}