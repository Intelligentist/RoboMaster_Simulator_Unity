using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandSupportBullet : MonoBehaviour {
    public bool Islandcheckinflag = false;
    public int Islandsupportbulletnum = 2;
    // Use this for initialization
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "IslandSupport")
        {
                Islandcheckinflag = true;
        }


    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "IslandSupport")
        {
            Islandcheckinflag = false;
        }
    }
}
