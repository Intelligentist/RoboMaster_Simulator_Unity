using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityStandardAssets;
using UnityStandardAssets.CrossPlatformInput;

public class InfantryFire : NetworkBehaviour {

        private float timeBetweenBullets;        // The time between each shot.
        public float MaxFrequence = 5;
        public int CurrentFrequence;
        [SyncVar]
        public float CurrentFiringRate;
        public float MaxRate = 16;
        public float MinRate = 14;
        [SyncVar]
        public int CurrentBulletnum = 0;
        private int Frequencenum;
        private float timer;
        private float FreTimer;                                
        public GameObject Bullet;
        private GameObject bullets;
        public Transform FirePoint1;
        public Transform FirePoint2;
        public Transform LaserPoint;
        private Rigidbody bulletsRig;
        private bool fireflag =false;
        [SyncVar]
        public Vector3 f;
        private Light Laser;
        public KeyCode key;
        public Transform MagzinePlate;
        public float smoothTime = 5f;
        [SyncVar]
        public bool openmagzineflag;
    private Quaternion MaxTargetRot;
    private Quaternion MinTargetRot;
    private RedSupportBullet Redsupportbullet;
    private BlueSupportBullet Bluesupportbullet;
    public InfantryCheckIn checkin1;
    public InfantryCheckIn checkin2;
    void Awake ()
        {
            Laser = LaserPoint.GetComponent<Light>();
            timeBetweenBullets = 1 / MaxFrequence;
            Redsupportbullet = GameObject.Find("RedCheckIn").GetComponent<RedSupportBullet>();
            Bluesupportbullet = GameObject.Find("BlueCheckIn").GetComponent<BlueSupportBullet>();
        MaxTargetRot.eulerAngles = new Vector3(0f, 90f, 0f);
        MinTargetRot.eulerAngles = new Vector3(0f, 0f, 0f);
    }



    void Update()
        {

            bool fire = Input.GetKey(KeyCode.F);
            CmdOpenMagzine();
            GetBullet();
            timer += Time.deltaTime;
            FreTimer += Time.deltaTime;
            if(FreTimer > 1.0f)
            {
                CurrentFrequence = Frequencenum;
                FreTimer = 0;
                Frequencenum = 0;
            }
            if (fire)
            {
                fireflag = !fireflag;
            }
            // Add the time since Update was last called to the timer.
            

#if !MOBILE_INPUT
            // If the Fire1 button is being press and it's time to fire...
            if (fireflag)
            {
                Laser.enabled = true;
                if (Input.GetButton("Fire1") && timer >= timeBetweenBullets && Time.timeScale != 0 && CurrentBulletnum > 0)
                {

                    CmdShoot();
                    CurrentBulletnum--;
                    Frequencenum++;
                    timer = 0f;
                }
            }
            else { Laser.enabled = false; }

#else
            // If there is input on the shoot direction stick and it's time to fire...
            if ((CrossPlatformInputManager.GetAxisRaw("Mouse X") != 0 || CrossPlatformInputManager.GetAxisRaw("Mouse Y") != 0) && timer >= timeBetweenBullets)
            {
                // ... shoot the gun
                //Shoot();
            }
#endif

        }

    [Command]
        void CmdShoot()
        {
            f = (FirePoint1.position - FirePoint2.position).normalized;
            CurrentFiringRate = Random.Range(MinRate, MaxRate);
            bullets = (GameObject)Instantiate(Bullet,FirePoint1.position,FirePoint1.rotation);
            bulletsRig=bullets.GetComponent<Rigidbody>();
            bulletsRig.velocity = f * CurrentFiringRate;
            Destroy(bullets,1.5f);
            NetworkServer.Spawn(bullets);    
        }

    [Command]
    void CmdOpenMagzine()
    {
        RpcOpenMagzine();
    }
    [ClientRpc]
    void RpcOpenMagzine()
    {
        bool magzine = Input.GetKey(key);
        if (magzine && MagzinePlate.localEulerAngles.y < 5f)
        {
            openmagzineflag = true;
        }
        if (magzine && MagzinePlate.localEulerAngles.y > 85f)
        {
            openmagzineflag = false;
        }
        if (openmagzineflag)
        {
            MagzinePlate.localRotation = Quaternion.Slerp(MagzinePlate.localRotation, MaxTargetRot,
            smoothTime * Time.deltaTime);
        }
        else
        {
            MagzinePlate.localRotation = Quaternion.Slerp(MagzinePlate.localRotation, MinTargetRot,
            smoothTime * Time.deltaTime);
        }
    }

    void GetBullet()
    {
            if (GetComponent<RobotInit>().robotinitcolor == Color.red)
            {
                if (Redsupportbullet.Redcheckinflag && openmagzineflag &&(checkin1.checkinflag|| checkin2.checkinflag))
                {
                    CurrentBulletnum += Redsupportbullet.Redsupportbulletnum;
                    Redsupportbullet.Redsupportrestbulletnum -= Redsupportbullet.Redsupportbulletnum;
                    Redsupportbullet.Redcheckinflag = false;
                }
            }

            if (GetComponent<RobotInit>().robotinitcolor == Color.blue)
            {
                if (Bluesupportbullet.Bluecheckinflag && openmagzineflag && (checkin1.checkinflag || checkin2.checkinflag))
                {
                    CurrentBulletnum += Bluesupportbullet.Bluesupportbulletnum;
                    Bluesupportbullet.Bluesupportrestbulletnum -= Bluesupportbullet.Bluesupportbulletnum;
                    Bluesupportbullet.Bluecheckinflag = false;
                }
            }
    }
        

    }
