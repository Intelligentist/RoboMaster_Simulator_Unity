
using UnityEngine;  
using System.Collections;  
  
public class DontDestroy : MonoBehaviour {  
    public bool DontDestroyOnLoad = true;  
    public bool DontCreateNewWhenBackToThisScene = true;
    public static DontDestroy Instance = null;  
    void Awake()  
    {  
        if(Instance != null)  
        {  
            GameObject.Destroy(this.gameObject);  
            return;  
        }  
        Instance = this;  
        if(this.DontDestroyOnLoad)  
            GameObject.DontDestroyOnLoad(this);  
    }  
    ///  
    /// 彻底独杜绝重新加载场景物体累加  
    ///  
    /* 
    static DontDestroyTool() 
    { 
    } 
    */  
}