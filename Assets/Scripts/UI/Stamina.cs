using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stamina : MonoBehaviour
{
    public Slider staminaSlider;
    public Image StaminaBar;

    public void SetMaxStaminaSlider(float value)
    {
        staminaSlider.maxValue = value; //set maxHP in script instead of inspector
        staminaSlider.value = value;  // set player's hp

        
    }

    public void setStaminaSlider(float value)
    {
        //staminaSlider.value = value;  //update player's health
        StaminaBar.fillAmount = value / 100;
        
    }
}
