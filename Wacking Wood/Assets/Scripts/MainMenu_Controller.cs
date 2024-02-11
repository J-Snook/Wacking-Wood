using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;

public class MainMenu_Controller : MonoBehaviour
{
    [Header("Volume Settings")] 
    [SerializeField] private TMP_Text volumeValue = null;
    [SerializeField] private Slider volumeSlider = null;
    [SerializeField] private float defaultVolume = 1.0f;
    [SerializeField] private GameObject confirmPrompt = null;
    
    [Header("Gameplay Settings")] 
    [SerializeField] private TMP_Text ControllerSenTextValue = null;
    [SerializeField] private Slider controllerSenSlider = null;
    [SerializeField] private int defaultSen = 4;
    public int mainControllerSen = 4;
    
    [Header("Load levels")] 
    public string _newGameLevel;
    private string levelToLoad;
    [SerializeField] private GameObject noSavedGame = null;


    // Levels Classes
    public void NewGameYes()
    {
        SceneManager.LoadScene(_newGameLevel); 
    }
    public void LoadGameYes()
    {
        if (PlayerPrefs.HasKey("SavedLevel"))
        {
            levelToLoad = PlayerPrefs.GetString("SavedLevel");
            SceneManager.LoadScene(levelToLoad);
        }
        else
        {
            noSavedGame.SetActive(true);
        }
    }
    public void ExitButton()
    {
        Application.Quit();
    }
    
    // Volume Classes

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        volumeValue.text = volume.ToString("0.0");
    }

    public void VolumeApply()
    {
        PlayerPrefs.SetFloat("Volume", AudioListener.volume);
        StartCoroutine(Confirmation());
    }

    public void SetControllerSen(float sensitivity)
    {
        mainControllerSen = Mathf.RoundToInt(sensitivity);
        ControllerSenTextValue.text = sensitivity.ToString("0");
        
    }

    public void GameApply()
    {
        PlayerPrefs.SetFloat("masterSen", mainControllerSen);
        StartCoroutine(Confirmation());
    }
    
    public void ResetButton(string MenuType)
    {
        if (MenuType == "Audio")
        {
            AudioListener.volume = defaultVolume;
            volumeSlider.value = defaultVolume;
            volumeValue.text = defaultVolume.ToString("0.0");
            VolumeApply();
        }

        if (MenuType == "Game")
        {
            ControllerSenTextValue.text = defaultSen.ToString("0");
            controllerSenSlider.value = defaultSen;
            mainControllerSen = defaultSen;
            GameApply();
            
        }
    }

    public IEnumerator Confirmation()
    {
        confirmPrompt.SetActive(true);
        yield return new WaitForSeconds(2);
        confirmPrompt.SetActive(false);
    }

}
    