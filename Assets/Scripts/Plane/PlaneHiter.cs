using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlaneHiter : MonoBehaviour {
    public int hitnum = 0;
    public float v;
    public float g;
    public float DropBlood;
    private GameObject bullets;
    private Rigidbody bulletRig;
    public GameObject blinklight;
    private float blinktime = 0f;
    private bool blinkflag = false;
    private bool deadflag = false;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (blinkflag && !deadflag)
        {
            blinktime += Time.deltaTime;


            if (blinktime >= 0.5f)
            {
                blinklight.SetActive(true);
                blinktime = 0;
                blinkflag = false;
            }
        }

    }

    void OnCollisionEnter(Collision Hiter)
    {


        v = Hiter.relativeVelocity.magnitude;
        bullets = Hiter.gameObject;
        bulletRig = bullets.GetComponent<Rigidbody>();
        g = bulletRig.mass;
        if (g == 0.05f && v > 12f)
        {
            DropBlood = 500f;
            hitnum++;
        }
        if (g == 0.05f && v < 12f)
        {
            DropBlood = 250f;
            hitnum++;
        }
        if (g == 0.01f)
        {
            DropBlood = 50f;
            hitnum++;
        }
        blink();
    }

    public void blink()
    {
        blinklight.SetActive(false);
        blinkflag = true;

    }



}

