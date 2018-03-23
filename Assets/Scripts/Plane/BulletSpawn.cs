using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class BulletSpawn : NetworkBehaviour
{

    public Transform BulletSpawnPoint;
    public GameObject LargeBullet;
	void Start () {
		
	}
	
	// Update is called once per frame
    public override void OnStartServer()
    {
        for (int i = 0; i < 10; i++)
        {
            GameObject Base = Instantiate(LargeBullet, BulletSpawnPoint.localPosition, BulletSpawnPoint.localRotation) as GameObject;
            NetworkServer.Spawn(Base);    
        }

	}
}


