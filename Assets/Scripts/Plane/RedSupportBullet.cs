using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RedSupportBullet : MonoBehaviour
{
    public bool Redcheckinflag = false;
    public int Redsupportbulletnum = 50;
    public int Redsupportrestbulletnum = 100;
    private float firstgivebulletstime = 1f;
    private float secondgivebulletstime = 2f;
    private float thirdgivebulletstime = 3f;
    // Use this for initialization


    private void Start()
    {
        StartCoroutine(FirstGiveBullets());
        StartCoroutine(SecondGiveBullets());
        StartCoroutine(ThirdGiveBullets());
    }
    void OnTriggerStay(Collider other)
    {

        if (other.gameObject.tag == "CheckIn")
        {
            if (Redsupportrestbulletnum > 0)
            {
                Redcheckinflag = true;               
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
    IEnumerator ThirdGiveBullets()
    {
        yield return new WaitForSeconds(thirdgivebulletstime * 60);
        Redsupportrestbulletnum += 100;
    }
}
