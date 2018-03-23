using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class BaseInstantiate : NetworkBehaviour
{
    public Transform BasePlane;
    public GameObject BaseModel;
    public bool GetStretch = false;
    public void Start()
    {
            Cmdbaseinstantiate();     
    }
    [Command]
    public void Cmdbaseinstantiate()
    {
        GameObject Base = Instantiate(BaseModel, BasePlane.localPosition, BasePlane.localRotation) as GameObject;
        Base.transform.parent = BasePlane.transform;
        NetworkServer.Spawn(Base);
    }



}   
