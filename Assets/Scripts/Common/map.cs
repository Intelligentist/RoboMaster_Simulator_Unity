using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class map : MonoBehaviour {
    public Transform HeroPostion;
    public RectTransform HeroMapPostion;
    Vector3 MapPostion;
	// Use this for initialization
	void Start () {


	}
	
	// Update is called once per frame
	void Update () {
        HeroMapPostion.localPosition = MapPostion / 13.6f * 80f;
        MapPostion = new Vector3(HeroPostion.localPosition.x, HeroPostion.localPosition.z, 0);
    }

}
