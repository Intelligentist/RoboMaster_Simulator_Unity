using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BaseDrive : NetworkBehaviour
{
    public Transform Base;
    public Transform Chassis;
    public float SpinSpeed = 1;
    [SyncVar]
     public float BaseYrot;
    [SyncVar]
    public Vector3 BaseTargetPos;
    [SyncVar]
    public float Timer;
    public float Length;
    [SyncVar]
    private float startTime;
    public float speed = 0.3F;
    private float journeyLength;
    private float DistancePerTime = 0.2f;
    public float speedfactor = 20;
    private BaseHealth basehealth;
    [SyncVar]
    public float fracJourney;
    [SyncVar]
    public float distCovered;

    // Use this for initialization
    void Start() {
        basehealth = GetComponent<BaseHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        if (basehealth.CurrentHealth != 0f)
        {
            if (Timer >= DistancePerTime)
            {
                Timer = 0;
                startTime = Time.time;
                BaseTargetPos = new Vector3(Random.Range(-Length, Length), 0.02f, Random.Range(-Length, Length));
                journeyLength = Vector3.Distance(Base.localPosition, BaseTargetPos);
                DistancePerTime = journeyLength / speed * speedfactor;
  
            }
            Timer += Time.deltaTime;
            CmdBaseSpin();

        }
    }

    [Command]
    void CmdBaseSpin()
    {
        RpcBaseSpin();
    }
    [ClientRpc]
    void RpcBaseSpin()
    {

        BaseYrot = SpinSpeed * Time.deltaTime * 10;
        Chassis.localRotation *= Quaternion.Euler(0f, BaseYrot, 0f);
        Base.localRotation = Quaternion.Euler(0f, 0f, 0f);

        distCovered = (Time.time - startTime) * speed;
        fracJourney = distCovered / journeyLength;
        Base.localPosition = Vector3.Lerp(Base.localPosition, BaseTargetPos, fracJourney);
    }

}
