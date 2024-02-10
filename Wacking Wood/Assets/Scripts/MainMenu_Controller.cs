using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class MainMenu_Controller : MonoBehaviour
{
    [Header("Volume Settings")] 
    [SerializeField] private TMP_Text volumeValue = null;
    [SerializeField] private Slider volumeSlider = null;
    [SerializeField] private GameObject confirmPrompt = null;
    
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
        StartCoroutine(VolumeConfirmation());
    }

    public IEnumerator VolumeConfirmation()
    {
        confirmPrompt.SetActive(true);
        yield return new WaitForSeconds(2);
        confirmPrompt.SetActive(false);
    }

}
    