using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Stretch1 : NetworkBehaviour
{
    public float Distance;
    public float Speed;
    public Transform StretchObj;
    private Vector3 Target;
    private Vector3 MaxTarget;
    private Vector3 MinTarget;
    private float startTime;

    public float Distance1;
    public float Speed1;
    public Transform StretchObj1;
    private Vector3 Target1;
    private Vector3 MaxTarget1;
    private Vector3 MinTarget1;
    private float startTime1;

    public KeyCode key;
    public KeyCode key1;
    private bool stretchflag = false;
    private bool stretchflag1 = false;
    [SyncVar]
    public bool GetStretch;
    [SyncVar]
    public bool GetDownFlag;
    public float PerDistance = 0.2f ;
    public GameObject camera1;
    public GameObject camera2;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        GetStretch = Input.GetKeyDown(key);
        GetDownFlag = Input.GetKey(key1);  
        CmdstretchY(GetStretch);    
    }








    [Command]
    void CmdstretchY(bool StretchFlag)
    {
        RpcstretchY(StretchFlag);
    }
    [ClientRpc]
    public void RpcstretchY(bool StretchFlag)
    {
        if (StretchFlag)
        {
            if (StretchObj1.localPosition.z > 0.9 * Distance1)
            {
                stretchflag = true;
            }
            if (StretchObj.localPosition.y < 0.1 * Distance)
            {
                stretchflag = false;
            }
            startTime = Time.time;
            startTime1 = Time.time;
            if (stretchflag)
            {
                Target = new Vector3(0, 0, 0);
                Target1 = new Vector3(0, 0, 0);
            }
            if (!stretchflag)
            {
                Target = new Vector3(0, Mathf.Abs(Distance), 0);
                Target1 = new Vector3(0, 0, Mathf.Abs(Distance1));
            }
        }
        if (stretchflag)
        {
            float distCovered1 = (Time.time - startTime1) * Speed1;
            float fracJourney1 = distCovered1 / Mathf.Abs(Distance1);
            StretchObj1.localPosition = Vector3.Lerp(StretchObj1.localPosition, Target1, fracJourney1);
            if (StretchObj1.localPosition.z < 0.1 * Distance1)
            {
                float distCovered = (Time.time - startTime) * Speed;
                float fracJourney = distCovered / Mathf.Abs(Distance);
                StretchObj.localPosition = Vector3.Lerp(StretchObj.localPosition, Target, fracJourney);
                camera1.SetActive(true);
                camera2.SetActive(false);
            }
        }
        if (!stretchflag)
        {
            float distCovered = (Time.time - startTime) * Speed;
            float fracJourney = distCovered / Mathf.Abs(Distance);
            StretchObj.localPosition = Vector3.Lerp(StretchObj.localPosition, Target, fracJourney);
            if (StretchObj.localPosition.y > 0.9 * Distance)
            {
                float distCovered1 = (Time.time - startTime1-1) * Speed1;
                float fracJourney1 = distCovered1 / Mathf.Abs(Distance1);
                StretchObj1.localPosition = Vector3.Lerp(StretchObj1.localPosition, Target1, fracJourney1);
                camera1.SetActive(false);
                camera2.SetActive(true);

            }
            if (GetDownFlag)
            {
                Target -= new Vector3(0f, PerDistance, 0f);
                if (Target.y<=2.0f)
                {
                    Target = new Vector3(0f, 2.0f, 0f);
                }
                float distCovered2 = (Time.time - startTime) * Speed;
                float fracJourney2 = distCovered2 / PerDistance;
                StretchObj.localPosition = Vector3.Lerp(StretchObj.localPosition, Target, fracJourney2);
            }


        }
    }




}
