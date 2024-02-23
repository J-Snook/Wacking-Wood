using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static bool pausedGame = false;

    public GameObject pausedMenuUI;
    public GameObject StaminaBarUI;
    public GameObject FuelBarUI;
    public GameObject CashTrackerUI;
    public GameObject TimeUI;
    public GameObject CursorUI;
    public Button resumeButton;
    public MonoBehaviour CharacterMovementScript;

    private void Start()
    {
        CharacterMovementScript = GetComponent("Character Movement") as MonoBehaviour;
        
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
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
       
    }
}
