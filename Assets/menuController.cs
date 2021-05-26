using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menuController : MonoBehaviour
{
    public GameObject canvas;
    public GameObject Layers;

    private void Awake()
    {
        GameObject screen = Instantiate<GameObject>(Layers);
        screen.transform.SetParent(canvas.transform, false);
        StartCoroutine(ShowLayer(screen));
    }

    private IEnumerator ShowLayer(GameObject screen)
    {
        yield return new WaitForSeconds(3f);
        screen.SetActive(true);
       // Destroy(screen);
    }
}
