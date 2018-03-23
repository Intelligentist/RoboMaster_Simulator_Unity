using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Vehicles.Car;
using UnityEngine.Networking;
public class InfantryFireLook : NetworkBehaviour
{
    [SyncVar]
    public float xRot;
    [SyncVar]
    public float yRot;

    public float XSensitivity = 2f;
    public float YSensitivity = 2f;
    private bool clampVerticalRotation = true;
    public float MinimumYaw = -45F;
    public float MaximumYaw = 45F;
    public float MinimumPitch = -20F;
    public float MaximumPitch = 20F;
    public bool smooth = true;
    public float smoothTime = 5f;
    [Range(1, 20)]
    [SerializeField]
    public float FollowRate = 10f;
    public bool lockCursor = true;
    public Transform Pitch;
    public Transform Yaw;
    public Transform Infantry;
    public GameObject PauseMeum;
    public float turnangle = 5f;
    public float turnsmoothtime = 2f;
    private float timer;
    private float deltaxRot;
    private Quaternion m_PitchTargetRot;
    [SyncVar]
    public Quaternion m_YawTargetRot;
    private Quaternion m_InfantryTargetRot;
    [SyncVar]
    public Quaternion m_InfantryCatWalkTargetRot;
    private bool m_cursorIsLocked = true;
    private Health InfantryHealth;
    public InfantryLockFire lockfire;
    [SyncVar]
    public bool catwalkflag;
    public float catwalkangle = 40;
    public float catwalksmoothtime = 5f;

    private float Timer;
    private int catwalkstep = 0;
    private float timeperstep = 1f;
    public float followtimefator = 5;
    public float followanglefator = 0.7f;
    public void Init()
    {
        m_PitchTargetRot = Pitch.localRotation;
        m_YawTargetRot = Yaw.localRotation;
        m_InfantryTargetRot = Infantry.localRotation;
        FollowRate = 1 / FollowRate;
    }


    [Command]
    public void CmdLookRotation(float yRot)
    {
        RpcLookRotation(yRot);
    }
    [ClientRpc]
    public void RpcLookRotation(float yRot)
    {


        m_PitchTargetRot *= Quaternion.Euler(yRot, 0f,0f );

        if (clampVerticalRotation)
        {
            m_PitchTargetRot = ClampRotationAroundXAxis(m_PitchTargetRot, MinimumPitch, MaximumPitch);
        }

        if (smooth)
        {
            Pitch.localRotation = Quaternion.Slerp(Pitch.localRotation, m_PitchTargetRot,
            smoothTime * Time.deltaTime);
        }
    }

    void LookRotation(float xRot)
    {
        if (xRot == 0f)
        {
            xRot = 0.001f;
        }
        m_YawTargetRot *= Quaternion.Euler(0f, xRot, 0f);
        m_InfantryTargetRot *= Quaternion.Euler(0f, xRot, 0f);
        if (clampVerticalRotation)
        {
            m_YawTargetRot = ClampRotationAroundYAxis(m_YawTargetRot, MinimumYaw, MaximumYaw);
            m_InfantryTargetRot = ClampRotationAroundZAxis(m_InfantryTargetRot, -100, 100);

        }
        if (smooth)
        {
            if (!lockfire.lockfireflag)
            {
                Infantry.localRotation = Quaternion.Slerp(Infantry.localRotation, m_InfantryTargetRot,
                smoothTime * Time.deltaTime);
                m_YawTargetRot = Quaternion.Euler(0f, 0f, 0f);
                Yaw.localRotation = Quaternion.Slerp(Yaw.localRotation, m_YawTargetRot,
                smoothTime * Time.deltaTime);
                GetComponent<InfantryCarControl>().enabled = true;

            }
            else
            {
                Yaw.localRotation = Quaternion.Slerp(Yaw.localRotation, m_YawTargetRot,
                    smoothTime * Time.deltaTime);
                GetComponent<InfantryCarControl>().enabled = false;
            }


        }
    }



    public void UpdateCursorLock()
    {
        //if the user set "lockCursor" we check & properly lock the cursos
        if (lockCursor)
            InternalLockUpdate();
    }

    private void InternalLockUpdate()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            m_cursorIsLocked = !m_cursorIsLocked;
        }


        if (m_cursorIsLocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            PauseMeum.SetActive(false);
            GetComponent<InfantryCarControl>().enabled = true;

        }
        else if (!m_cursorIsLocked)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            PauseMeum.SetActive(true);
            GetComponent<InfantryCarControl>().enabled = false;
        }
    }

    Quaternion ClampRotationAroundXAxis(Quaternion q, float MinimumX, float MaximumX)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);
        angleX = Mathf.Clamp(angleX, MinimumX, MaximumX);
        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);
        return q;
    }



    Quaternion ClampRotationAroundYAxis(Quaternion q, float MinimumY, float MaximumY)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;
        float angleY = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.y);
        angleY = Mathf.Clamp(angleY, MinimumY, MaximumY);
        q.y = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleY);
        return q;
    }

    Quaternion ClampRotationAroundZAxis(Quaternion q, float MinimumZ, float MaximumZ)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;
        float angleZ = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.z);
        angleZ = Mathf.Clamp(angleZ, MinimumZ, MaximumZ);
        q.z = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleZ);
        return q;
    }


    void Turn()
    {
        bool turnleft = Input.GetKey(KeyCode.Q);
        bool turnright = Input.GetKey(KeyCode.E);
        if (turnleft)
        {
            m_InfantryTargetRot *= Quaternion.Euler(0f, -turnangle, 0f);
            Infantry.localRotation = Quaternion.Slerp(Infantry.localRotation, m_InfantryTargetRot,
             turnsmoothtime * Time.deltaTime);
        }
        if (turnright)
        {
            m_InfantryTargetRot *= Quaternion.Euler(0f, turnangle, 0f);
            Infantry.localRotation = Quaternion.Slerp(Infantry.localRotation, m_InfantryTargetRot,
             turnsmoothtime * Time.deltaTime);
        }
    }
    [Command]
    void CmdCatWalk(bool catwalkflag, Quaternion m_YawTargetRot,Quaternion m_InfantryCatWalkTargetRot)
    {
        RpcCatWalk(catwalkflag,m_YawTargetRot,m_InfantryCatWalkTargetRot);
    }

    [ClientRpc]
    void RpcCatWalk(bool catwalkflag, Quaternion m_YawTargetRot, Quaternion m_InfantryCatWalkTargetRot)
    {
        if (catwalkflag)
        {
            if (catwalkstep == 0)
            {
                m_YawTargetRot = Quaternion.Euler(0f, 0f, 0f);
                Yaw.localRotation = Quaternion.Slerp(Yaw.localRotation, m_YawTargetRot,
                catwalksmoothtime * followtimefator * Time.deltaTime);
                m_InfantryCatWalkTargetRot = m_InfantryTargetRot * Quaternion.Euler(0f, 0f, 0f);
                Infantry.localRotation = Quaternion.Slerp(Infantry.localRotation, m_InfantryCatWalkTargetRot,
                catwalksmoothtime * Time.deltaTime);

            }
            if (catwalkstep == 1)
            {
                m_YawTargetRot = Quaternion.Euler(0f, catwalkangle * followanglefator, 0f);
                Yaw.localRotation = Quaternion.Slerp(Yaw.localRotation, m_YawTargetRot,
                catwalksmoothtime * followtimefator * Time.deltaTime);
                m_InfantryCatWalkTargetRot = m_InfantryTargetRot * Quaternion.Euler(0f, -catwalkangle, 0f);
                Infantry.localRotation = Quaternion.Slerp(Infantry.localRotation, m_InfantryCatWalkTargetRot,
                catwalksmoothtime * Time.deltaTime);

            }

            if (catwalkstep == 2)
            {
                m_YawTargetRot = Quaternion.Euler(0f, -catwalkangle * followanglefator, 0f);
                Yaw.localRotation = Quaternion.Slerp(Yaw.localRotation, m_YawTargetRot,
                catwalksmoothtime * followtimefator * Time.deltaTime);
                m_InfantryCatWalkTargetRot = m_InfantryTargetRot * Quaternion.Euler(0f, catwalkangle, 0f);
                Infantry.localRotation = Quaternion.Slerp(Infantry.localRotation, m_InfantryCatWalkTargetRot,
                catwalksmoothtime * Time.deltaTime);

            }
        }
        //else
        //{
        //    m_YawTargetRot = Quaternion.Euler(0f, 0f, 0f);
        //    Yaw.localRotation = Quaternion.Slerp(Yaw.localRotation, m_YawTargetRot,
        //    catwalksmoothtime * followtimefator * Time.deltaTime);
        //    m_InfantryCatWalkTargetRot = m_InfantryTargetRot * Quaternion.Euler(0f, 0f, 0f);
        //    Infantry.localRotation = Quaternion.Slerp(Infantry.localRotation, m_InfantryCatWalkTargetRot,
        //    catwalksmoothtime * Time.deltaTime);
        //}
    }

    void Start()
    {
        Init();
        InfantryHealth = GetComponent<Health>();
    }
    void Update()
    {
        if (m_cursorIsLocked)
        {
            xRot = CrossPlatformInputManager.GetAxis("Mouse X") * XSensitivity;
            yRot = -CrossPlatformInputManager.GetAxis("Mouse Y") * YSensitivity;
            catwalkflag = Input.GetButton("Fire2");
            CmdLookRotation(yRot);
            LookRotation(xRot);
            if (catwalkflag)
            {
                Timer += Time.deltaTime;
                if (Timer >= 2 * timeperstep)
                {
                    Timer = 0;
                }
                catwalkstep = (int)(Timer / timeperstep + 1);

            }
            else
            {
                Timer = 0;
                catwalkstep = 0;
            }
            Turn();
            //CmdCatWalk(catwalkflag,m_YawTargetRot,m_InfantryCatWalkTargetRot);
        }
        UpdateCursorLock();
    }





}