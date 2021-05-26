using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HP3D : MonoBehaviour
{
    public Slider HpSlider;
    public Gradient gradient;
    public Image HPBar;

    public void SetMaxHP(float health)
    {
        HpSlider.maxValue = health; //set maxHP in script instead of inspector
        HpSlider.value = health;  // set player's hp

        //gradient.Evaluate(1f); // set color for different Hp %
    }

    public void setHealth(float health)
    {
        //HpSlider.value = health;  //update player's health
        HPBar.fillAmount = health / 100;
        //HPBar.color = gradient.Evaluate(HpSlider.normalizedValue); //set the HPBar color match with the gradient we've set
    }


}
