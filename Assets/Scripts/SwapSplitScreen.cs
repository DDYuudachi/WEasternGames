using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script to swap between a vertical split screen and a horizontal one
/// </summary>
public class SwapSplitScreen : MonoBehaviour
{
    public Camera p1Camera, p2Camera;

    private bool _isHorizontal = true;
     
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            _isHorizontal = !_isHorizontal;

            SwapSplit();
        }
    }

    private void SwapSplit()
    {
        if (_isHorizontal)
        {
            p1Camera.rect = new Rect(0f, .5f, 1f, .5f);
            p2Camera.rect = new Rect(0f, 0f, 1f, .5f);
        }
        else
        {
            p1Camera.rect = new Rect(0f, 0f, .5f, 1f);
            p2Camera.rect = new Rect(.5f, 0f, .5f, 1f);
        }
    }
}
