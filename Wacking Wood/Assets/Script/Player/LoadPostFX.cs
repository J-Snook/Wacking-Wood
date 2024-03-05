using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class LoadPostFX : MonoBehaviour
{
    [SerializeField] private PostProcessVolume postProcessingVolume;
    [SerializeField] private Bloom bloom;
    [SerializeField] private MotionBlur motionBlur;
    void Start()
    {
        postProcessingVolume.profile.TryGetSettings(out bloom);
        postProcessingVolume.profile.TryGetSettings(out motionBlur);

        bloom.active = PlayerPrefs.GetInt("masterBloom") == 1;
        motionBlur.active = PlayerPrefs.GetInt("masterMotionBlur") == 1;
    }
}
