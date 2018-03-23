using System.Collections;
using System.Collections.Generic;


using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;


internal enum colorchoose
{
    RED,
    BLUE,
}
internal enum carchoose
{
    HERO,
    ENGINEER,
    INFANTRY,
}
internal enum spawnchoose
{
    spawn1,
    spawn2,
    spawn3,
    spawn4,
    spawn5,
    spawn6,
}
public class UIbutton : NetworkBehaviour
{
    public Button Red;
    public Button Blue;
    public Button Hero;
    public Button Engineer;
    public Button Infantry;
    public Button[] Spawn = new Button[6];
    private Button[] SpawnButton = new Button[6];
    public GameObject Colour;
    public GameObject robot;
    public GameObject spawnpoint;
    public Transform[] realspawnpoint = new Transform[12];
    private colorchoose color;  
    private carchoose car;
    private spawnchoose spawn;
    public Transform HeroModel;
    public Transform EngineerModel;
    public Transform InfantryModel;
    public bool SpawnFlag = false;
    public GameObject Map;
    // Use this for initialization
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Button RedButton = Red.GetComponent<Button>();
        Button BuleButton = Blue.GetComponent<Button>();
        Button HeroButton = Hero.GetComponent<Button>();
        Button EngineerButton = Engineer.GetComponent<Button>();
        Button InfantryButton = Infantry.GetComponent<Button>();
        for (int i = 0; i < 6; i++)
        {
            SpawnButton[i] = Spawn[i].GetComponent<Button>();
        }

        RedButton.onClick.AddListener(RedButtonTaskOnClick);
        BuleButton.onClick.AddListener(BuleButtonTaskOnClick);
        HeroButton.onClick.AddListener(HeroButtonTaskOnClick);
        EngineerButton.onClick.AddListener(EngineerButtonTaskOnClick);
        InfantryButton.onClick.AddListener(InfantryTaskOnClick);
        SpawnButton[0].onClick.AddListener(SpawnButton1);
        SpawnButton[1].onClick.AddListener(SpawnButton2);
        SpawnButton[2].onClick.AddListener(SpawnButton3);
        SpawnButton[3].onClick.AddListener(SpawnButton4);
        SpawnButton[4].onClick.AddListener(SpawnButton5);
        SpawnButton[5].onClick.AddListener(SpawnButton6);


    }

    // Update is called once per frame
    void Update()
    {
        if (SpawnFlag)
        {

            InstantiateModel();
            Map.SetActive(false);
            SpawnFlag = false;
        }
    }

    void RedButtonTaskOnClick()
    {
        Colour.SetActive(false);
        robot.SetActive(true);
        color = colorchoose.RED;

    }


    void BuleButtonTaskOnClick()
    {
        Colour.SetActive(false);
        robot.SetActive(true);
        color = colorchoose.BLUE;
    }

    void HeroButtonTaskOnClick()
    {
        robot.SetActive(false);
        spawnpoint.SetActive(true);        
        car = carchoose.HERO;

    }

    void EngineerButtonTaskOnClick()
    {
        robot.SetActive(false);
        spawnpoint.SetActive(true); 
        car = carchoose.ENGINEER;
    }

    void InfantryTaskOnClick()
    {
        robot.SetActive(false);
        spawnpoint.SetActive(true); 
        car = carchoose.INFANTRY;
    }

    void SpawnButton1()
    {
        spawnpoint.SetActive(false);
        spawn = spawnchoose.spawn1;
        SpawnFlag = true;
    }
    void SpawnButton2()
    {
        spawnpoint.SetActive(false);
        spawn = spawnchoose.spawn2;
        SpawnFlag = true;
    }
    void SpawnButton3()
    {
        spawnpoint.SetActive(false);
        spawn = spawnchoose.spawn3;
        SpawnFlag = true;
    }
    void SpawnButton4()
    {
        spawnpoint.SetActive(false);
        spawn = spawnchoose.spawn4;
        SpawnFlag = true;
    }
    void SpawnButton5()
    {
        spawnpoint.SetActive(false);
        spawn = spawnchoose.spawn5;
        SpawnFlag = true;
    }
    void SpawnButton6()
    {
        spawnpoint.SetActive(false);
        spawn = spawnchoose.spawn6;
        SpawnFlag = true;
    }

    void InstantiateModel()
    {
        switch (color)
        {
            case colorchoose.RED: switch (car)
                {
                    case carchoose.HERO: switch (spawn)
                        {
				case spawnchoose.spawn1: Instantiate(HeroModel, realspawnpoint[0].localPosition, realspawnpoint[0].localRotation);
                                break;
				case spawnchoose.spawn2: Instantiate(HeroModel, realspawnpoint[1].localPosition, realspawnpoint[1].localRotation);
                                break;
				case spawnchoose.spawn3: Instantiate(HeroModel, realspawnpoint[2].localPosition, realspawnpoint[2].localRotation);
                                break;
				case spawnchoose.spawn4: Instantiate(HeroModel, realspawnpoint[3].localPosition, realspawnpoint[3].localRotation);
                                break;
				case spawnchoose.spawn5: Instantiate(HeroModel, realspawnpoint[4].localPosition, realspawnpoint[4].localRotation);
                                break;
				case spawnchoose.spawn6: Instantiate(HeroModel, realspawnpoint[5].localPosition, realspawnpoint[5].localRotation);
                                break;
                            default:
                                break;
                        }
                        break;
                    case carchoose.ENGINEER: switch (spawn)
                        {
				case spawnchoose.spawn1: Instantiate(EngineerModel, realspawnpoint[0].localPosition, realspawnpoint[0].localRotation);
                                break;
				case spawnchoose.spawn2: Instantiate(EngineerModel, realspawnpoint[1].localPosition, realspawnpoint[1].localRotation);
                                break;
				case spawnchoose.spawn3: Instantiate(EngineerModel, realspawnpoint[2].localPosition, realspawnpoint[2].localRotation);
                                break;
				case spawnchoose.spawn4: Instantiate(EngineerModel, realspawnpoint[3].localPosition, realspawnpoint[3].localRotation);
                                break;
				case spawnchoose.spawn5: Instantiate(EngineerModel, realspawnpoint[4].localPosition, realspawnpoint[4].localRotation);
                                break;
				case spawnchoose.spawn6: Instantiate(EngineerModel, realspawnpoint[5].localPosition, realspawnpoint[5].localRotation);
                                break;
                            default:
                                break;
                        }
                        break;
                    case carchoose.INFANTRY: switch (spawn)
                        {
				case spawnchoose.spawn1: Instantiate(InfantryModel, realspawnpoint[0].localPosition, realspawnpoint[0].localRotation);
                                break;
				case spawnchoose.spawn2: Instantiate(InfantryModel, realspawnpoint[1].localPosition, realspawnpoint[1].localRotation);
                                break;
				case spawnchoose.spawn3: Instantiate(InfantryModel, realspawnpoint[2].localPosition, realspawnpoint[2].localRotation);
                                break;
				case spawnchoose.spawn4: Instantiate(InfantryModel, realspawnpoint[3].localPosition, realspawnpoint[3].localRotation);
                                break;
				case spawnchoose.spawn5: Instantiate(InfantryModel, realspawnpoint[4].localPosition, realspawnpoint[4].localRotation);
                                break;
				case spawnchoose.spawn6: Instantiate(InfantryModel, realspawnpoint[5].localPosition, realspawnpoint[5].localRotation);
                                break;
                            default:
                                break;
                        }
                        break;
                    default:
                        break;
                }
                break;
            case colorchoose.BLUE: switch (car)
                {
                    case carchoose.HERO: switch (spawn)
                        {
				case spawnchoose.spawn1: Instantiate(HeroModel, realspawnpoint[6].localPosition, realspawnpoint[6].localRotation);
                                break;
				case spawnchoose.spawn2: Instantiate(HeroModel, realspawnpoint[7].localPosition, realspawnpoint[7].localRotation);
                                break;
				case spawnchoose.spawn3: Instantiate(HeroModel, realspawnpoint[8].localPosition, realspawnpoint[8].localRotation);
                                break;
				case spawnchoose.spawn4: Instantiate(HeroModel, realspawnpoint[9].localPosition, realspawnpoint[9].localRotation);
                                break;
				case spawnchoose.spawn5: Instantiate(HeroModel, realspawnpoint[10].localPosition, realspawnpoint[10].localRotation);
                                break;
				case spawnchoose.spawn6: Instantiate(HeroModel, realspawnpoint[11].localPosition, realspawnpoint[11].localRotation);
                                break;
                            default:
                                break;
                        }
                        break;
                    case carchoose.ENGINEER: switch (spawn)
                        {
				case spawnchoose.spawn1: Instantiate(EngineerModel, realspawnpoint[6].localPosition, realspawnpoint[6].localRotation);
                                break;
				case spawnchoose.spawn2: Instantiate(EngineerModel, realspawnpoint[7].localPosition, realspawnpoint[7].localRotation);
                                break;
				case spawnchoose.spawn3: Instantiate(EngineerModel, realspawnpoint[8].localPosition, realspawnpoint[8].localRotation);
                                break;
				case spawnchoose.spawn4: Instantiate(EngineerModel, realspawnpoint[9].localPosition, realspawnpoint[9].localRotation);
                                break;
				case spawnchoose.spawn5: Instantiate(EngineerModel, realspawnpoint[10].localPosition, realspawnpoint[10].localRotation);
                                break;
				case spawnchoose.spawn6: Instantiate(EngineerModel, realspawnpoint[11].localPosition, realspawnpoint[11].localRotation);
                                break;
                            default:
                                break;
                        }
                        break;
                    case carchoose.INFANTRY: switch (spawn)
                        {
                            case spawnchoose.spawn1: Instantiate(InfantryModel, realspawnpoint[6].localPosition, realspawnpoint[6].localRotation);
                                break;
                            case spawnchoose.spawn2: Instantiate(InfantryModel, realspawnpoint[7].localPosition, realspawnpoint[7].localRotation);
                                break;
                            case spawnchoose.spawn3: Instantiate(InfantryModel, realspawnpoint[8].localPosition, realspawnpoint[8].localRotation);
                                break;
                            case spawnchoose.spawn4: Instantiate(InfantryModel, realspawnpoint[9].localPosition, realspawnpoint[9].localRotation);
                                break;
                            case spawnchoose.spawn5: Instantiate(InfantryModel, realspawnpoint[10].localPosition, realspawnpoint[10].localRotation);
                                break;
                            case spawnchoose.spawn6: Instantiate(InfantryModel, realspawnpoint[11].localPosition, realspawnpoint[11].localRotation);
                                break;
                            default:
                                break;
                        }
                        break;
                    default:
                        break;
                }
                break;
            default:
                break;
        }
    }

}


