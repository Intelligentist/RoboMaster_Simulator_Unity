using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class RobotInit : NetworkBehaviour
{
    public Material Red;
    public Material Blue;
    public MeshRenderer[] Light = new MeshRenderer[10];
    public Color robotinitcolor;
    public string playername;
    public string robotname;
    // Use this for initialization
    void Start() {

        StartCoroutine(ChangeColor());
        
    }

    // Update is called once per frame
    void Update() {
        
    }
    IEnumerator ChangeColor()
    {
        yield return new WaitForSeconds(0.05f);
        Cmdchangecolor();
    }
    [Command]
    void Cmdchangecolor()
    {
        Rpcchangecolor();
    }
    [ClientRpc]
    void Rpcchangecolor()
    {
        if (robotinitcolor == Color.red)
        {
            for (int i = 0; i < 10; i++)
            {

             
            }
        }
        if (robotinitcolor == Color.blue)
        {
            for (int i = 0; i < 10; i++)
            {
                if (Light[i].material != null)
                {
                    Light[i].material = Blue;
                }
            }
        }
    }
}
