using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupportBullet : MonoBehaviour {
    public bool Redcheckinflag =false;
    public int Redsupportbulletnum = 50;
    private int Redsupportrestbulletnum = 200;
    private float firstgivebulletstime = 1f;
    private float secondgivebulletstime = 3f;
    // Use this for initialization


    private void Start()
    {
        StartCoroutine(FirstGiveBullets());
        StartCoroutine(SecondGiveBullets());
    }
    void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "CheckIn")
        {
            if (Redsupportrestbulletnum >= 0)
            {
                Redcheckinflag = true;
                Redsupportrestbulletnum -= 50;
            }
            
        }


    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "CheckIn")
        {
            Redcheckinflag = false;
        }
    }
    IEnumerator FirstGiveBullets()
    {
        yield return new WaitForSeconds(firstgivebulletstime * 60);
        Redsupportrestbulletnum += 100;
    }
    IEnumerator SecondGiveBullets()
    {
        yield return new WaitForSeconds(secondgivebulletstime * 60);
        Redsupportrestbulletnum += 100;
    }
}
