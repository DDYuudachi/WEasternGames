using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class titlePage : MonoBehaviour
{
    public GameObject loadingScreen;
    public Slider slider;
    public Text progressPercent;

    void Start()
    {
        //Set Cursor to not be visible
        Cursor.visible = true;
    }

    public void startGame(int sceneIndex)
    {
        if (Input.anyKeyDown)
        {
            //https://www.youtube.com/watch?v=YMj2qPq9CP8
            StartCoroutine(LoadAsynchronously(sceneIndex)); //
        }
    }

    //void Update()
    //{

    //    if (Input.anyKeyDown)
    //    {
    //        SceneManager.LoadScene("newMainMenu");
    //        Debug.Log("A key click has been detected");
    //    }
    //    //if (Input.GetMouseButtonDown(0))
    //    //{
    //    //    SceneManager.LoadScene("mainMenu 1");
    //    //    Debug.Log("mouse click has been detected");
    //    //}
    //}

    IEnumerator LoadAsynchronously(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex); // to know how the loading operation is going

        loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            //Debug.Log(operation.progress); // the output of load level will stop at 0.9, because unity do the loading in 0 - 0.9 and active the level and drop the things that dont need in 0.9 - 1.0
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            //Debug.Log(progress); //this will get output to 1 when the loading progress is finished
            slider.value = progress;
            progressPercent.text = Mathf.Round(progress * 100) + "%";
            yield return null; //wait until goes to the next frame before continuing
        }
    }
}
