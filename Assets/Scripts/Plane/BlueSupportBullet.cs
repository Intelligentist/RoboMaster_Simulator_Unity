using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueSupportBullet : MonoBehaviour
{
    public bool Bluecheckinflag = false;
    public int Bluesupportbulletnum = 50;
    public int Bluesupportrestbulletnum = 100;
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
            if (Bluesupportrestbulletnum >= 0)
            {
                Bluecheckinflag = true;
            }

        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "CheckIn")
        {
            Bluecheckinflag = false;
        }
    }
    IEnumerator FirstGiveBullets()
    {
        yield return new WaitForSeconds(firstgivebulletstime * 60);
        Bluesupportrestbulletnum += 100;
    }
    IEnumerator SecondGiveBullets()
    {
        yield return new WaitForSeconds(secondgivebulletstime * 60);
        Bluesupportrestbulletnum += 100;
    }
    IEnumerator ThirdGiveBullets()
    {
        yield return new WaitForSeconds(thirdgivebulletstime * 60);
        Bluesupportrestbulletnum += 100;
    }
}

