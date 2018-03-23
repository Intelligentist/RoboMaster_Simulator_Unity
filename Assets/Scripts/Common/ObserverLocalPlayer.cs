using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.Characters.FirstPerson;
public class ObserverLocalPlayer : NetworkBehaviour
{
    public GameObject maincamera;
    void Start () {
        if (!isLocalPlayer)
        {
            maincamera.GetComponent<Camera>().enabled = false;
            GetComponent<FirstPersonController>().enabled = false;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
