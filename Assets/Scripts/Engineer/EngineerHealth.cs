using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class EngineerHealth : NetworkBehaviour
{


    public float FullHealth = 5000;
    private float pastCurrentHealth;
    [SyncVar]
    public float CurrentHealth = 5000;
    public Transform mask;
    public GameObject BloodBackLight;
    public HitCounter LeftHit;
    public HitCounter RightHit;
    private bool KillFlag = false;
    private float SumHit;
    private EngineerLocalPlayer dead;
    void Start()
    {
        CurrentHealth = FullHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<RobotInit>().robotinitcolor == Color.blue)
        {
            KillFlag = GameObject.Find("RedCheckPlane").GetComponent<RedSupportKill>().RedKillflag;
        }

        if (GetComponent<RobotInit>().robotinitcolor == Color.red)
        {
            KillFlag = GameObject.Find("BlueCheckPlane").GetComponent<BlueSupportKill>().BlueKillflag;
        }
        SumHit = LeftHit.hitnum * LeftHit.DropBlood + RightHit.hitnum * RightHit.DropBlood;
        CurrentHealth = FullHealth - SumHit;
        if (pastCurrentHealth != CurrentHealth)
        {
            CmdBloodLight(CurrentHealth);
            pastCurrentHealth = CurrentHealth;
        }
        if (CurrentHealth <= 0 || KillFlag == true)
        {
            Dead();
        }
    }

    [Command]
    void CmdBloodLight(float health)
    {
        RpcBloodLight(health);
    }

    [ClientRpc]
    void RpcBloodLight(float health)
    {
        mask.localScale = new Vector3(1, 1, -(1 - health / FullHealth));
    }


    [Command]
    void CmdBloodLightOff()
    {
        RpcBloodLightOff();
    }
    [ClientRpc]
    void RpcBloodLightOff()
    {
        BloodBackLight.SetActive(false);
    }

    [Command]
    void CmdBlinkLightOff()
    {
        RpcBlinkLightOff();
    }
    [ClientRpc]
    void RpcBlinkLightOff()
    {
        LeftHit.blinklight.SetActive(false);
        RightHit.blinklight.SetActive(false);
    }



    void Dead()
    {
        CurrentHealth = 0;
        CmdBloodLightOff();
        CmdBlinkLightOff();
        dead.Uncontrol();
    }

}
