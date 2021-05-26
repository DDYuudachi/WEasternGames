using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    public float DestroyTime = 0.3f;
    public Camera camera;
    public Vector3 RandomIntensity = new Vector3(0.5f, 0, 0);

    void Start()
    {
       //Destroy(gameObject, DestroyTime);

        transform.localPosition += new Vector3(Random.Range(RandomIntensity.x, RandomIntensity.x), Random.Range(RandomIntensity.y, RandomIntensity.y),
            Random.Range(RandomIntensity.z, RandomIntensity.z));
    }

    private void Update()
    {
        DestroyTime -= Time.deltaTime;
        if (DestroyTime <= 0) {
            Object.Destroy(gameObject);
        }
        transform.LookAt(camera.transform.position);
        transform.eulerAngles += Vector3.up * 180;
    }


}
