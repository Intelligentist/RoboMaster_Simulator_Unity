using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseHitCounter : MonoBehaviour
{
    public int hitnum = 0;
    public float v;
    public float g;
    public float DropBlood;
    private GameObject bullets;
    private Rigidbody bulletRig;
    public GameObject blinklight;
    private float blinktime = 0f;
    public bool Hitflag = false;

    void Update()
    {
        if (Hitflag)
        {
            blinktime += Time.deltaTime;
            if (blinktime >= 0.5f)
            {
                blinklight.SetActive(true);
                blinktime = 0;
                Hitflag = false;
            }
        }
    }


    void OnCollisionEnter(Collision Hiter)
    {
        hitnum++;
        v = Hiter.relativeVelocity.magnitude;
        bullets = Hiter.gameObject;
        bulletRig = bullets.GetComponent<Rigidbody>();
        g = bulletRig.mass;
        if (g == 0.05f && v > 12f)
        {
            DropBlood = 400f;
        }
        if (g == 0.05f && v < 12f)
        {
            DropBlood = 150f;
        }
        if (g == 0.01f)
        {
            DropBlood = 100f;
        }
        blinklight.SetActive(false);
        Hitflag = true;
    }





}
