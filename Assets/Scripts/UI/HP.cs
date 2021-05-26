using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HP : MonoBehaviour
{
    public Slider HpSlider;
   
    public Image HPBar;

    public void SetMaxHP(float health)
    {
        HpSlider.maxValue = health; //set maxHP in script instead of inspector
        HpSlider.value = health;  // set player's hp

       
    }
    
    public void setHealth(float health)
    {
        HPBar.fillAmount = health / 100;

    }

    
}
