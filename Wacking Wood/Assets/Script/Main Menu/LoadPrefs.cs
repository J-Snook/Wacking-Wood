using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadPrefs : MonoBehaviour
{
 [Header("General Settings")] 
 [SerializeField] private bool canUse = false;
 [SerializeField] private MainMenu_Controller menuController;
 
 [Header("Volume Settings")]
 [SerializeField] private TMP_Text volumeValue = null;
 [SerializeField] private Slider volumeSlider = null;
 
 [Header("Brightness Settings")]
 [SerializeField] private TMP_Text brightnessValue = null;
 [SerializeField] private Slider brightnessSlider = null;
 
 [Header("Quality Settings")]
 [SerializeField] private TMP_Dropdown qualityDropdown;
 
 [Header("Fullscreen Settings")]
 [SerializeField] private Toggle fullScreenToggle;

 [Header("Bloom Settings")] 
 [SerializeField] private Toggle bloomToggle;

 [Header("Motion Blur Settings")]
 [SerializeField] private Toggle motionBlurToggle;
 
 [Header("Sensitivity Settings")]
 [SerializeField] private TMP_Text controllerSenTextValue = null;
 [SerializeField] private Slider controllerSenSlider = null;


 private void Awake()
 {
     if (canUse)
     {
         if (PlayerPrefs.HasKey("masterVolume"))
         {
             float localVolume = PlayerPrefs.GetFloat("masterVolume");

             volumeValue.text = localVolume.ToString("0.0");
             volumeSlider.value = localVolume;
             AudioListener.volume = localVolume;
         }
         else
         {
             menuController.ResetButton("Audio");
         }

         if (PlayerPrefs.HasKey("masterQuality"))
         {
             int localQuality = PlayerPrefs.GetInt("masterQuality");
             qualityDropdown.value = localQuality;
             QualitySettings.SetQualityLevel(localQuality);
         }

         if (PlayerPrefs.HasKey("masterFullScreen"))
         {
             int localFullscreen = PlayerPrefs.GetInt("masterFullscreen");

             if (localFullscreen == 1)
             {
                 Screen.fullScreen = true;
                 fullScreenToggle.isOn = true;
             }
             else
             {
                 Screen.fullScreen = false;
                 fullScreenToggle.isOn = false;
             }
         }

         if (PlayerPrefs.HasKey("masterBrightness"))
         {
             float localBrightness = PlayerPrefs.GetFloat("masterBrightness");

             brightnessValue.text = localBrightness.ToString("0.0");
             brightnessSlider.value = localBrightness;
         }

         if (PlayerPrefs.HasKey("masterSen"))
         {
             float localSensitivity = PlayerPrefs.GetFloat("masterSen");

             controllerSenTextValue.text = localSensitivity.ToString("0");
             controllerSenSlider.value = localSensitivity;
             menuController.mainControllerSen = Mathf.RoundToInt(localSensitivity);
         }

         if (PlayerPrefs.HasKey("masterBloon"))
         {
             bool bloom = PlayerPrefs.GetInt("masterBloom") == 1;
             bloomToggle.isOn = bloom;
         }

         if (PlayerPrefs.HasKey("masterMotionBlur"))
         {
            bool blur = PlayerPrefs.GetInt("masterMotionBlur") == 1;
            motionBlurToggle.isOn = blur;
         }
     }
 }
}
