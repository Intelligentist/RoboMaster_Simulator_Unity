using UnityEngine;
using UnityEngine.Networking;
using Prototype.NetworkLobby;
using System.Collections;
using System.Collections.Generic;
public class BaseHealth : NetworkBehaviour
{


    public float FullHealth = 5000;
    [SyncVar]
    public float CurrentHealth = 5000;
    public Transform mask;
    public BaseHitCounter LeftHit;
    public BaseHitCounter RightHit;
    public BaseHitCounter FrontHit;
    public BaseHitCounter BackHit;
    public BaseHitCounter UpHit1;
    public BaseHitCounter UpHit2;
    private float SumHit;

    void Start()
    {
        CurrentHealth = FullHealth;
    }

    // Update is called once per frame
    [ServerCallback]
    void Update()
    {
        SumHit = LeftHit.hitnum * LeftHit.DropBlood + RightHit.hitnum * RightHit.DropBlood + FrontHit.hitnum * FrontHit.DropBlood + BackHit.hitnum * BackHit.DropBlood + UpHit1.hitnum * UpHit1.DropBlood + UpHit2.hitnum * UpHit2.DropBlood;
        CurrentHealth = FullHealth - SumHit;
        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            RpcBlinkLightOff();
            StartCoroutine(ReturnToLoby());

        }
        CmdBloodLight();
    }

    IEnumerator ReturnToLoby()
    {
        yield return new WaitForSeconds(3.0f);
        LobbyManager.s_Singleton.ServerReturnToLobby();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }


    [Command]
    void CmdBloodLight()
    {
        RpcBloodLight();
    }
    [ClientRpc]
    void RpcBloodLight()
    {
        mask.localScale = new Vector3(1, 1, -(1 - CurrentHealth / FullHealth));
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
        BackHit.blinklight.SetActive(false);
        UpHit1.blinklight.SetActive(false);
        UpHit2.blinklight.SetActive(false);
    }




}

