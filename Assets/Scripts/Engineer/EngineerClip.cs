using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class EngineerClip : NetworkBehaviour
{
    public Transform RotateObj1;
    public Transform RotateObj2;
    public KeyCode key;
    private Quaternion MaxQuaternion1;
    private Quaternion MinQuaternion1;
    private Quaternion MaxQuaternion2;
    private Quaternion MinQuaternion2;
    public float MaxAngle;
    public float MinAngle;
    [SyncVar]
    public bool rotateFlag = false;
    public bool HasRotateFlag;
    public float smoothTime = 5f;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        rotateFlag = Input.GetKey(key);
        CmdRotateZ(rotateFlag);
    }


    [Command]
    void CmdRotateZ(bool rotateFlag)
    {
        RpcRotateZ(rotateFlag);
    }
    [ClientRpc]
    void RpcRotateZ(bool rotateFlag)
    {
        if (rotateFlag && RotateObj1.localEulerAngles.z < MinAngle + 5f)
        {
            HasRotateFlag = true;
        }
        if (rotateFlag && RotateObj1.localEulerAngles.z > MaxAngle - 5f)
        {
            HasRotateFlag = false;
        }
        if (HasRotateFlag)
        {
            MaxQuaternion1.eulerAngles = new Vector3(0, 0, MaxAngle);
            MaxQuaternion2.eulerAngles = new Vector3(0, 0, -MaxAngle);
            RotateObj1.localRotation = Quaternion.Slerp(RotateObj1.localRotation, MaxQuaternion1,
            smoothTime * Time.deltaTime);
            RotateObj2.localRotation = Quaternion.Slerp(RotateObj2.localRotation, MaxQuaternion2,
smoothTime * Time.deltaTime);
        }
        else
        {
            MinQuaternion1.eulerAngles = new Vector3(0, 0, MinAngle);
            MinQuaternion2.eulerAngles = new Vector3(0, 0, -MinAngle);
            RotateObj1.localRotation = Quaternion.Slerp(RotateObj1.localRotation, MinQuaternion1,
            smoothTime * Time.deltaTime);
            RotateObj2.localRotation = Quaternion.Slerp(RotateObj2.localRotation, MinQuaternion2,
smoothTime * Time.deltaTime);
        }
    }

}
