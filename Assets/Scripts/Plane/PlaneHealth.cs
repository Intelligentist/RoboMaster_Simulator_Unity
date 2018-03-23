using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlaneHealth : NetworkBehaviour
{
    public float FullHealth = 5000;
    public float CurrentHealth = 5000;
    public PlaneHiter LeftHit;
    public PlaneHiter RightHit;
    public PlaneHiter FrontHit;
    private float SumHit;
    // Use this for initialization
    void Start()
    {
        CurrentHealth = FullHealth;
    }

    // Update is called once per frame
    void Update()
    {
        {
            SumHit = LeftHit.hitnum * LeftHit.DropBlood + RightHit.hitnum * RightHit.DropBlood + FrontHit.hitnum * FrontHit.DropBlood;
            CurrentHealth = FullHealth - SumHit;
            if (CurrentHealth <= 0)
            {
                CurrentHealth = 0;
            }
            if (CurrentHealth == 0f)
            {
                CmdBlinkLightOff();
            }
        }
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
        FrontHit.blinklight.SetActive(false);
    }


}

