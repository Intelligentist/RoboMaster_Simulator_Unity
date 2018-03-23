using UnityEngine.Networking;using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EngineerStretch : NetworkBehaviour
{
    public float PerDistance = 0.1f;
    public float Speed;
    public Transform StretchObj;
    [SyncVar]
    public Vector3 Target;
    public float MaxTarget;
    public float MinTarget;
    [SyncVar]
    public float startTime;
    [SyncVar]
    private bool Up;
    private bool Down;
    [SyncVar]
    public bool SecondFlag;

    public float PerDistance1 = 0.1f;
    public float Speed1;
    public Transform StretchObj1;
    [SyncVar]
    public Vector3 Target1 ;
    public float MaxTarget1;
    public float MinTarget1;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Up = Input.GetButton("Fire1");
        Down = Input.GetButton("Fire2");
        SecondFlag = Input.GetKey(KeyCode.Z);
        CmdStrenth();

    }

    [Command]
    void CmdStrenth()
    {
        RpcStrenth();
    }
    [ClientRpc]
    void RpcStrenth()
    {
        if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2"))
        {
            startTime = Time.time;
        }
        if (SecondFlag)
        {
            if (Up)
            {
                Target += new Vector3(0f, PerDistance, 0f);
            }
            if (Down)
            {
                Target -= new Vector3(0f, PerDistance, 0f);
            }
            if (Target.y >= MaxTarget)
            {
                Target.y = MaxTarget;
            }
            if (Target.y <= MinTarget)
            {
                Target.y = MinTarget;
            }
            float distCovered = (Time.time - startTime) * Speed;
            float fracJourney = distCovered / PerDistance;
            StretchObj.localPosition = Vector3.Lerp(StretchObj.localPosition, Target, fracJourney);
        }


        if (!SecondFlag)
        {
            if (Up)
            {
                Target1 += new Vector3(0f, PerDistance1, 0f);
            }
            if (Down)
            {
                Target1 -= new Vector3(0f, PerDistance1, 0f);
            }

            if (Target1.y >= MaxTarget1)
            {
                Target1.y = MaxTarget1;
            }
            if (Target1.y <= MinTarget1)
            {
                Target1.y = MinTarget1;
            }
            float distCovered1 = (Time.time - startTime) * Speed1;
            float fracJourney1 = Mathf.Abs(distCovered1 / PerDistance1);
            StretchObj1.localPosition = Vector3.Lerp(StretchObj1.localPosition, Target1, fracJourney1);
        }
    }

}
