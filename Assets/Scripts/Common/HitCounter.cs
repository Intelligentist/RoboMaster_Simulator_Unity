using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HitCounter : MonoBehaviour
{

    public int hitnum = 0;
    public float v;
    public float g;
    public float DropBlood;
    private GameObject bullets;
    private Rigidbody bulletRig;
    public Image HitBlood;
    public GameObject blinklight;
    private float blinktime = 0f;
    public bool Hitflag = false;
    public GameObject Robot;
    private RobotInit robotinit;
    public Color robotcolor;
    public float robothealth = 0.0f;
    public InfantryHealth infantryhealth;
    public Health herohealth;
    void Start()
    {
        robotinit = Robot.GetComponent<RobotInit>();
    }
    void Update()
    {
        
        robotcolor = robotinit.robotinitcolor;
        if (herohealth != null)
        {           
            robothealth = herohealth.CurrentHealth;
        }
        if (infantryhealth !=null)
        {
            robothealth = infantryhealth.CurrentHealth;
        }


        if (Hitflag)
        {
            blinktime += Time.deltaTime;
            if (blinktime >= 0.3f)
            {
                HitBlood.enabled = false;
                blinklight.SetActive(true);
                blinktime = 0;
                Hitflag = false;
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
            blinklight.SetActive(false);
            UIBlink();
            Hitflag = true;
        }
        if (g == 0.05f && v < 12f)
        {
            DropBlood = 250f;
            hitnum++;
            blinklight.SetActive(false);
            UIBlink();
            Hitflag = true;
        }
        if (g == 0.01f )
        {
            DropBlood = 50f;
            hitnum++;
            blinklight.SetActive(false);
            UIBlink();
            Hitflag = true;
        }
        if (g == 0.02f)
        {
            DropBlood = 50f;
            hitnum++;
            blinklight.SetActive(false);
            UIBlink();
            Hitflag = true;
        }
    }

    void UIBlink()
    {
        HitBlood.enabled = true;
        HitBlood.color = new Color(1f, 1f, 1f, 0.3f);
    }




}
