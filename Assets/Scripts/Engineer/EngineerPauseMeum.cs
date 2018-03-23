using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EngineerPauseMeum : MonoBehaviour
{
    public Slider YawSensitivity;
    public Slider PitchSensitivity;
    public EngineerFireLook firelook;
    public float XSensitivity;
    public float YSensitivity;
    public Button Exit;
    // Use this for initialization
    void Start()
    {
        XSensitivity = firelook.XSensitivity;
        YSensitivity = firelook.YSensitivity;
    }

    // Update is called once per frame
    void Update()
    {
        YawSensitivity.onValueChanged.AddListener(delegate { YawValueChangeCheck(); });
        PitchSensitivity.onValueChanged.AddListener(delegate { PitchValueChangeCheck(); });
        Exit.onClick.AddListener(ExitGame);
    }

    public void YawValueChangeCheck()
    {
        firelook.XSensitivity = XSensitivity * YawSensitivity.value / 2f;
    }
    public void PitchValueChangeCheck()
    {
        firelook.YSensitivity = YSensitivity * PitchSensitivity.value / 2f;
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
