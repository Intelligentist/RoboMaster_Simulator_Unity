using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Stretch2 : NetworkBehaviour
{
    public float Distance;
    public float Speed;
    public Transform StretchObj;
    public Aixs axis;
    public KeyCode key;
    private Vector3 Target;
    private float startTime;
    private bool stretchflag = false;
    [SyncVar]
    public bool GetStretch;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        GetStretch = Input.GetKey(key);
        ChooseAxisStretch();
    }

    void ChooseAxisStretch()
    {
        switch (axis)
        {
            case Aixs.X: CmdstretchX(GetStretch);
                break;
            case Aixs.Y: CmdstretchY(GetStretch);
                break;
            case Aixs.Z: CmdstretchZ(GetStretch);
                break;
            default:
                break;
        }
    }



    [Command]
    void CmdstretchX(bool StretchFlag)
    {
        RpcstretchX(StretchFlag);
    }
    [ClientRpc]
    public void RpcstretchX(bool StretchFlag)
    {
        if (StretchFlag)
        {
            if (StretchObj.localPosition.x > 0.5 * Distance)
            {
                stretchflag = true;
            }
            if (StretchObj.localPosition.x < 0.5 * Distance)
            {
                stretchflag = false;
            }
            startTime = Time.time;
            if (stretchflag)
            { Target = StretchObj.localPosition + new Vector3(-Mathf.Abs(Distance), 0, 0); }
            if (!stretchflag)
            { Target = StretchObj.localPosition + new Vector3(Mathf.Abs(Distance), 0, 0); }
        }
        if (stretchflag)
        {
            float distCovered = (Time.time - startTime) * Speed;
            float fracJourney = distCovered / Mathf.Abs(Distance);
            StretchObj.localPosition = Vector3.Lerp(StretchObj.localPosition, Target, fracJourney);
        }
        if (!stretchflag)
        {
            float distCovered = (Time.time - startTime) * Speed;
            float fracJourney = distCovered / Mathf.Abs(Distance);
            StretchObj.localPosition = Vector3.Lerp(StretchObj.localPosition, Target, fracJourney);
        }
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
            if (StretchObj.localPosition.y > 0.5 * Distance)
            {
                stretchflag = true;
            }
            if (StretchObj.localPosition.y < 0.5 * Distance)
            {
                stretchflag = false;
            }
            startTime = Time.time;
            if (stretchflag)
            { Target = StretchObj.localPosition + new Vector3(0, -Mathf.Abs(Distance), 0); }
            if (!stretchflag)
            { Target = StretchObj.localPosition + new Vector3(0, Mathf.Abs(Distance), 0); }
        }
        if (stretchflag)
        {
            float distCovered = (Time.time - startTime) * Speed;
            float fracJourney = distCovered / Mathf.Abs(Distance);
            StretchObj.localPosition = Vector3.Lerp(StretchObj.localPosition, Target, fracJourney);
        }
        if (!stretchflag)
        {
            float distCovered = (Time.time - startTime) * Speed;
            float fracJourney = distCovered / Mathf.Abs(Distance);
            StretchObj.localPosition = Vector3.Lerp(StretchObj.localPosition, Target, fracJourney);
        }
    }



    [Command]
    void CmdstretchZ(bool StretchFlag)
    {
        RpcstretchZ(StretchFlag);
    }
    [ClientRpc]
    public void RpcstretchZ(bool StretchFlag)
    {
        if (StretchFlag)
        {
            if (StretchObj.localPosition.z > 0.5 * Distance)
            {
                stretchflag = true;
            }
            if (StretchObj.localPosition.z < 0.5 * Distance)
            {
                stretchflag = false;
            }
            startTime = Time.time;
            if (stretchflag)
            { Target = StretchObj.localPosition + new Vector3(0, 0, -Mathf.Abs(Distance)); }
            if (!stretchflag)
            { Target = StretchObj.localPosition + new Vector3(0, 0, Mathf.Abs(Distance)); }
        }
        if (stretchflag)
        {
            float distCovered = (Time.time - startTime) * Speed;
            float fracJourney = distCovered / Mathf.Abs(Distance);
            StretchObj.localPosition = Vector3.Lerp(StretchObj.localPosition, Target, fracJourney);
        }
        if (!stretchflag)
        {
            float distCovered = (Time.time - startTime) * Speed;
            float fracJourney = distCovered / Mathf.Abs(Distance);
            StretchObj.localPosition = Vector3.Lerp(StretchObj.localPosition, Target, fracJourney);
        }
    }

}
