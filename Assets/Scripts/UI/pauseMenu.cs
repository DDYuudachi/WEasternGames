using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class pauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    private int currentSceneIndex;

    public GameObject pauseMenuUI;

    void Start()
    {
        //Set Cursor to not be visible
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        AudioListener.pause = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        AudioListener.pause = true;
    }

    public void Menu()
    {
        Time.timeScale = 1f;
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        PlayerPrefs.SetInt("SavedScene", currentSceneIndex);
        SceneManager.LoadScene(1);
    }

    //public void LoadMenu()
    //{
        
    //    Time.timeScale = 1f;
    //    currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    //    PlayerPrefs.SetInt("SavedScene", currentSceneIndex);
    //    SceneManager.LoadScene(0);
    //}

    public void QuitGame()
    {
       Application.Quit();
    }
}
