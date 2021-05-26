using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonSound : MonoBehaviour
{
    public AudioSource button;
    public AudioClip hoverEffect;
    public AudioClip clickEffect;

    public void HoverSound()
    {
        button.PlayOneShot(hoverEffect);
    }

    public void ClickSound()
    {
        button.PlayOneShot(clickEffect);
    }
}
