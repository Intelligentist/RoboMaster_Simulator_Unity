using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BaseFire : NetworkBehaviour
{

    private float timeBetweenBullets;        // The time between each shot.
    public float MaxFrequence = 5;
    public int CurrentFrequence;
    [SyncVar]
    public float CurrentFiringRate;
    public float MaxRate = 16;
    public float MinRate = 14;
    public int bulletsnum = 0;
    public int Maxbulletsnum = 500;
    private int Frequencenum;
    private float timer;
    private float FreTimer;
    public GameObject Bullet;
    private GameObject bullets;
    public Transform FirePoint1;
    public Transform FirePoint2;
    public Transform LaserPoint;
    private Rigidbody bulletsRig;
    private bool fireflag = false;
    private Light Laser;
    [SyncVar]
    public Vector3 f;

    public Transform Pitch;
    public Transform Yaw;
    [SyncVar]
    private Vector3 FireHiterVector;
    private Vector3 YawTargetVet;
    [SyncVar]
    private Quaternion YawTarget;
    [SyncVar]
    private Quaternion PitchTarget;
    private Vector3 PitchTargetVet;
    public float YawSmoothTime = 5;
    public float PitchSmoothTime = 5;
    public float FireMinDis;
    [SyncVar]
    private float YawAngle;
    private float pastYawAngle;
    [SyncVar]
    private float PitchAngle;
    private RedFireRange RedBaseFlag;
    private BlueFireRange BlueBaseFlag;
    private BaseHealth basehealth;
    private bool BaseFlag =false;
    private Vector3 BaseLeftHiterPos;
    private Vector3 BaseRightHiterPos;
    private Vector3 BaseFrontHiterPos;
    private Vector3 BaseBackHiterPos;

    void Awake()
    {
        Laser = LaserPoint.GetComponent<Light>();
        timeBetweenBullets = 1 / MaxFrequence;



        basehealth = GetComponent<BaseHealth>();
    }


    void Update()
    {
        if (GetComponent<Rigidbody>().name == "RedBase(Clone)")
        {
            RedBaseFlag = GameObject.Find("RedFireRange").GetComponent<RedFireRange>();
            BaseFlag = RedBaseFlag.BaseFireFlag;
            BaseLeftHiterPos = RedBaseFlag.LeftHiterPos;
            BaseRightHiterPos = RedBaseFlag.RightHiterPos;
            BaseFrontHiterPos = RedBaseFlag.FrontHiterPos;
            BaseBackHiterPos = RedBaseFlag.BackHiterPos;
        }
        if (GetComponent<Rigidbody>().name == "BlueBase(Clone)")
        {
            BlueBaseFlag = GameObject.Find("BlueFireRange").GetComponent<BlueFireRange>();
            BaseFlag = BlueBaseFlag.BaseFireFlag;
            BaseLeftHiterPos = BlueBaseFlag.LeftHiterPos;
            BaseRightHiterPos = BlueBaseFlag.RightHiterPos;
            BaseFrontHiterPos = BlueBaseFlag.FrontHiterPos;
            BaseBackHiterPos = BlueBaseFlag.BackHiterPos;
        }

        if (basehealth.CurrentHealth !=0f)
        {
            CompareDistance(BaseLeftHiterPos, BaseRightHiterPos, BaseFrontHiterPos, BaseBackHiterPos);
            YawTargetVet = new Vector3(FireHiterVector.x, 0, FireHiterVector.z);
            pastYawAngle = YawAngle;
            YawAngle = Vector3.Angle(Vector3.forward, YawTargetVet);
            YawAngle *= Mathf.Sign(Vector3.Cross(Vector3.forward, YawTargetVet).y);
            if (YawAngle < 0)
            {
                YawAngle = 360 + YawAngle;
            }
            if (Mathf.Abs(pastYawAngle - YawAngle) < 180f)
            {
                RpcYaw();
            }
            RpcPitch();

            bool fire = BaseFlag;
            timer += Time.deltaTime;
            FreTimer += Time.deltaTime;
            if (FreTimer > 1.0f)
            {
                CurrentFrequence = Frequencenum;
                FreTimer = 0;
                Frequencenum = 0;
            }
            if (fire)
            {
                fireflag = !fireflag;
            }
            if (fireflag)
            {
                Laser.enabled = true;
                if (BaseFlag && timer >= timeBetweenBullets && Time.timeScale != 0 && bulletsnum <= Maxbulletsnum)
                {

                    CmdShoot();
                    bulletsnum++;
                    Frequencenum++;
                    timer = 0f;
                }
            }
            else { Laser.enabled = false; } 
        }
 

    }

    [Command]
    void CmdShoot()
    {
        f = (FirePoint1.position - FirePoint2.position).normalized;
        CurrentFiringRate = Random.Range(MinRate, MaxRate);
        bullets = (GameObject)Instantiate(Bullet, FirePoint1.position, FirePoint1.rotation);
        bulletsRig = bullets.GetComponent<Rigidbody>();
        bulletsRig.velocity = f * CurrentFiringRate;
        Destroy(bullets, 1f);
        NetworkServer.Spawn(bullets);
    }

    [ClientRpc]
    void RpcPitch()
    {
        PitchAngle = -Vector3.Angle(YawTargetVet, FireHiterVector);
        PitchAngle *= Mathf.Sign(Vector3.Cross(YawTargetVet, FireHiterVector).x);
        PitchTarget = Quaternion.Euler(PitchAngle, 0f, 0f);
        Pitch.localRotation = Quaternion.Slerp(Pitch.localRotation, PitchTarget,
        PitchSmoothTime * Time.deltaTime);
    }

    [ClientRpc]
    void RpcYaw()
    {
        YawTarget = Quaternion.Euler(0f, YawAngle, 0f);
        Yaw.localRotation = Quaternion.Slerp(Yaw.localRotation, YawTarget,
        YawSmoothTime * Time.deltaTime);
    }


    void CompareDistance(Vector3 Left, Vector3 Right, Vector3 Front, Vector3 Back)
    {
        float LeftDis = (Left - Pitch.position).magnitude;
        float RightDis = (Right - Pitch.position).magnitude;
        float FrontDis = (Front - Pitch.position).magnitude;
        float BackDis = (Back - Pitch.position).magnitude;
        FireMinDis =Mathf.Min(LeftDis, RightDis, FrontDis, BackDis);
        if (FireMinDis == LeftDis)
        {
            FireHiterVector = Left - Pitch.position;
        }
        if (FireMinDis == RightDis)
        {
            FireHiterVector = Right - Pitch.position;
        }
        if (FireMinDis == FrontDis)
        {
            FireHiterVector = Front - Pitch.position;
        }
        if (FireMinDis == BackDis)
        {
            FireHiterVector = Back - Pitch.position;
        }
    }

}
