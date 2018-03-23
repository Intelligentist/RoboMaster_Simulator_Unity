using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class Rotate : NetworkBehaviour
{
    public Aixs axis;
    public Transform RotateObj;
    public KeyCode key;
    private Quaternion MaxQuaternion;
    private Quaternion MinQuaternion;
    public float MaxAngle;
    public float MinAngle;
    [SyncVar]
    public bool rotateFlag =false;
    public bool HasRotateFlag;
    public float smoothTime = 5f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        rotateFlag = Input.GetKey(key);
        ChooseAxisRotate();
	}
    void ChooseAxisRotate()
    {
        switch (axis)
        {
            case Aixs.X: CmdRotateX(rotateFlag);
                break;
            case Aixs.Y: CmdRotateY(rotateFlag);
                break;
            case Aixs.Z: CmdRotateZ(rotateFlag);
                break;
            default:
                break;
        }
    }

    [Command]
    void CmdRotateX(bool rotateFlag)
    { 
        RpcRotateX(rotateFlag);
    }
    [ClientRpc]
    void RpcRotateX(bool rotateFlag)
    {
        if (rotateFlag && RotateObj.localEulerAngles.x < MinAngle +5f)
        {
            HasRotateFlag = true;
        }
        if (rotateFlag && RotateObj.localEulerAngles.x > MaxAngle -5f)
        {
            HasRotateFlag = false;
        }
        if (HasRotateFlag)
        {
            MaxQuaternion.eulerAngles = new Vector3(MaxAngle,0,0);
            RotateObj.localRotation = Quaternion.Slerp(RotateObj.localRotation, MaxQuaternion,
            smoothTime * Time.deltaTime);
        }
        else
        {
            MinQuaternion.eulerAngles = new Vector3(MinAngle, 0, 0);
            RotateObj.localRotation = Quaternion.Slerp(RotateObj.localRotation, MinQuaternion,
            smoothTime * Time.deltaTime);
        }
    }

    [Command]
    void CmdRotateY(bool rotateFlag)
    {
        RpcRotateY(rotateFlag);
    }
    [ClientRpc]
    void RpcRotateY(bool rotateFlag)
    {
        if (rotateFlag && RotateObj.localEulerAngles.y < MinAngle + 5f)
        {
            HasRotateFlag = true;
        }
        if (rotateFlag && RotateObj.localEulerAngles.y > MaxAngle - 5f)
        {
            HasRotateFlag = false;
        }
        if (HasRotateFlag)
        {
            MaxQuaternion.eulerAngles = new Vector3(0, MaxAngle, 0);
            RotateObj.localRotation = Quaternion.Slerp(RotateObj.localRotation, MaxQuaternion,
            smoothTime * Time.deltaTime);
        }
        else
        {
            MinQuaternion.eulerAngles = new Vector3(0, MinAngle, 0);
            RotateObj.localRotation = Quaternion.Slerp(RotateObj.localRotation, MinQuaternion,
            smoothTime * Time.deltaTime);
        }
    }


    [Command]
    void CmdRotateZ(bool rotateFlag)
    {
        RpcRotateZ(rotateFlag);
    }
    [ClientRpc]
    void RpcRotateZ(bool rotateFlag)
    {
        if (rotateFlag && RotateObj.localEulerAngles.z < MinAngle + 5f)
        {
            HasRotateFlag = true;
        }
        if (rotateFlag && RotateObj.localEulerAngles.z > MaxAngle - 5f)
        {
            HasRotateFlag = false;
        }
        if (HasRotateFlag)
        {
            MaxQuaternion.eulerAngles = new Vector3(0, 0, MaxAngle);
            RotateObj.localRotation = Quaternion.Slerp(RotateObj.localRotation, MaxQuaternion,
            smoothTime * Time.deltaTime);
        }
        else
        {
            MinQuaternion.eulerAngles = new Vector3(0, 0, MinAngle);
            RotateObj.localRotation = Quaternion.Slerp(RotateObj.localRotation, MinQuaternion,
            smoothTime * Time.deltaTime);
        }
    }

}
