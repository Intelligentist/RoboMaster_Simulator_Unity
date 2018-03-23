using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using  UnityStandardAssets.Vehicles.Car;
public class paramters : MonoBehaviour
{
    public Text speed;
    public Text frequence;
    public Text FiringRate;
    public Text health;
    public Text bulletnum;
    public Text time;
    public CarController CarSpeed;
    public Fire CarFrequence;
    public Health CarHealth;
    public int MatchTime = 7;
    private double timer;
    private double RestTime;
    private int min;
    private int sec;
    // Use this for initialization
    void Start()
    {
        StartCoroutine(TimeStart());
    }
    IEnumerator TimeStart()
    {
        yield return new WaitForSeconds(0.05f);
    }
    // Update is called once per frame
    void Update()
    {
        speed.text = CarSpeed.CurrentSpeed.ToString("F1");
        frequence.text = CarFrequence.CurrentFrequence.ToString();
        FiringRate.text = CarFrequence.CurrentFiringRate.ToString("F1");
        health.text = CarHealth.CurrentHealth.ToString();
        bulletnum.text = CarFrequence.CurrentBulletnum.ToString();
        timer = Time.realtimeSinceStartup;
        RestTime = MatchTime * 60 - timer;
        min = (int)(RestTime / 60);
        sec = (int)(RestTime - min * 60);
        if (sec < 10)
        { time.text = min.ToString() + ":0" + sec.ToString(); }
        else {  time.text = min.ToString() + ':' + sec.ToString(); }
    }
}