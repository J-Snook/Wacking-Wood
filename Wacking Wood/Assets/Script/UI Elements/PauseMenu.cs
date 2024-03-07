using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool pausedGame = false;

    public GameObject pauseMenuContainer;
    public GameObject StaminaBarUI;
    public GameObject FuelBarUI;
    public GameObject CashTrackerUI;
    public GameObject TimeUI;
    public GameObject CursorUI;
    public GameObject HotBarUI;
    
    public Button resumeButton;

    private string mainSceneName = "JoshWorkingScene";


    private void Start()
    {
        
        
        resumeButton.onClick.AddListener(ResumeButton);
        
        
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pausedGame)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    public void MainMenubutton()
    {
        SceneManager.LoadSceneAsync("MainMenu");
        pausedGame = false;
        Time.timeScale = 1f;
    }
    private void Resume()
    {

        Time.timeScale = 1f;
        pausedGame = false;
        pauseMenuContainer.SetActive(false);

        //enables all the bars
        StaminaBarUI.SetActive(true);
        FuelBarUI.SetActive(true);
        CashTrackerUI.SetActive(true);
        TimeUI.SetActive(true);
        CursorUI.SetActive(true);
        HotBarUI.SetActive(true);


        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    public void ResumeButton()
    {
        Resume();
    }

    private void Pause()
    {

        Time.timeScale = 0f;
        pausedGame = true;

        pauseMenuContainer.SetActive(true);
        StaminaBarUI.SetActive(false);
        FuelBarUI.SetActive(false);
        CashTrackerUI.SetActive(false);
        TimeUI.SetActive(false);
        CursorUI.SetActive(false);
        HotBarUI.SetActive(false);





        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;


    }
}
