using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static bool pausedGame = false;

    public GameObject pausedMenuUI;
    public GameObject pauseMenuContainer;
    public GameObject StaminaBarUI;
    public GameObject FuelBarUI;
    public GameObject CashTrackerUI;
    public GameObject TimeUI;
    public GameObject CursorUI;
    public GameObject PauseMenu_Container;
    public GameObject SettingsMenu_Container;
    public GameObject GraphicsPanel_Dialoug;
    public GameObject SoundPanel_Dialoug;
    public GameObject GamePanel_Dialoug;
    public Button resumeButton;

    private Quaternion originalRotation;
   

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

    private void Resume()
    {
        pausedMenuUI.SetActive(false);
        Time.timeScale = 1f;
        pausedGame = false;
        
        //enables all the bars
        StaminaBarUI.SetActive(true);
        FuelBarUI.SetActive(true);
        CashTrackerUI.SetActive(true);
        TimeUI.SetActive(true);
        CursorUI.SetActive(true);
        PauseMenu_Container.SetActive(false);
        SettingsMenu_Container.SetActive(false);
        GraphicsPanel_Dialoug.SetActive(false);
        SoundPanel_Dialoug.SetActive(false);
        GamePanel_Dialoug.SetActive(false);


        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    public void ResumeButton()
    {
        Resume();
    }

    private void Pause()
    {
        pausedMenuUI.SetActive(true);
        Time.timeScale = 0f;
        pausedGame = true;

        StaminaBarUI.SetActive(false);
        FuelBarUI.SetActive(false);
        CashTrackerUI.SetActive(false);
        TimeUI.SetActive(false);
        CursorUI.SetActive(false);
        PauseMenu_Container.SetActive(true);
        SettingsMenu_Container.SetActive(false);
        GraphicsPanel_Dialoug.SetActive(false);
        SoundPanel_Dialoug.SetActive(false);
        GamePanel_Dialoug.SetActive(false);
        



        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;


    }
}
