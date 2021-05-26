using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePreload : MonoBehaviour
{
    //https://docs.unity3d.com/ScriptReference/Resources.LoadAll.html
    public static Object[] images;  

    void Start()
    {
        images = Resources.LoadAll("Debug_Combat&Movement/Paer_Prototype_UI", typeof(Texture)); // any png file will be classified as texture
        foreach (var obj in images)
        {
            //Debug.Log(obj.name);
        }
    }
}
