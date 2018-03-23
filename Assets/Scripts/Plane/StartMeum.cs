using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMeum : MonoBehaviour {
    public float time;
    public GameObject startmeum;
    public GameObject Map;
    public GameObject Colour;
	// Use this for initialization
	void Start () {
        startmeum.SetActive(true);

	}
	
	// Update is called once per frame
	void Update () {
        time = Time.realtimeSinceStartup;
        if (time > 3f && time < 4f)
        {
            startmeum.SetActive(false);
             Map.SetActive(true);
            Colour.SetActive(true);
        }


	}
}
