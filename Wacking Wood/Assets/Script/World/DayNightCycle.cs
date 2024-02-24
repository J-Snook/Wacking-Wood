using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [SerializeField] private Cycles[] cycles;
    [SerializeField] private float numSecondsPerMinute = 1f;
    [SerializeField] private float deltaTimeMultipler = 1f;
    [SerializeField] private Light globalLight;
    [SerializeField] private PlayerUI _playerUI;


    private int minutes;
    public int Minutes { get { return minutes; } set { minutes = value;OnMinuteChange(value); } }
    private int hours;
    public int Hours { get { return hours; } set { hours = value; OnHourChange(value); } }
    private int days;
    public int Days { get { return days; } set {days=value; } }

    private float tempSeconds;
    private Gradient[] gradients;

    private void Start()
    {
        globalLight.color = cycles[0].color;
        RenderSettings.skybox.SetTexture("_Texture1", cycles[0].tex);
        RenderSettings.skybox.SetFloat("_Blend", 0);
        gradients = new Gradient[cycles.Length];
        GradientAlphaKey[] alphas = new GradientAlphaKey[2] { new GradientAlphaKey(1f, 1f), new GradientAlphaKey(1f, 1f) };
        for(int i = 0; i < cycles.Length-1; i++)
        {
            gradients[i] = new Gradient();
            GradientColorKey[] colourKey = new GradientColorKey[2] { new GradientColorKey(cycles[i].color, 0f), new GradientColorKey(cycles[i+1].color, 10f) };
            gradients[i].SetKeys(colourKey, alphas);
        }
        gradients[cycles.Length-1] = new Gradient();
        GradientColorKey[] FinalcolourKey = new GradientColorKey[2] { new GradientColorKey(cycles[cycles.Length - 1].color, 0f), new GradientColorKey(cycles[0].color, 10f) };
        gradients[cycles.Length - 1].SetKeys(FinalcolourKey, alphas);
    }

    public void Update()
    {
        tempSeconds += Time.deltaTime * deltaTimeMultipler;
        if (tempSeconds >= numSecondsPerMinute)
        {
            Minutes++;
            tempSeconds -= numSecondsPerMinute;
        }
        _playerUI.UpdateTime(Hours, Minutes);
    }

    private void OnMinuteChange(int value)
    {
        globalLight.transform.Rotate(Vector3.up, (1f / 1440f) * 360f, Space.World);
        if (value >= 60)
        {
            Hours++;
            Minutes = 0;
        }
    }

    private void OnHourChange(int value)
    {
        if(value >= 24)
        {
            Days++;
            Hours = 0;
        }
        for(int i = 0; i < cycles.Length; i++)
        {
            if(cycles[i].hourTime==Hours)
            {
                StartCoroutine(LerpSkybox(cycles[i].tex, cycles[(i+1 >= cycles.Length) ? 0 : i+1].tex, 10f));
                StartCoroutine(LerpLight(i, 10f));
            }
        }
    }

    private IEnumerator LerpSkybox(Texture2D from, Texture2D to, float time)
    {
        RenderSettings.skybox.SetTexture("_Texture1",from);
        RenderSettings.skybox.SetTexture("_Texture2", to);
        RenderSettings.skybox.SetFloat("_Blend", 0);
        for(float i = 0; i < time; i+=Time.deltaTime)
        {
            RenderSettings.skybox.SetFloat("_Blend", i / time);
            yield return null;
        }
        RenderSettings.skybox.SetTexture("_Texture1", to);
    }

    private IEnumerator LerpLight(int index,float time)
    {
        for(float i = 0; i < time; i += Time.deltaTime)
        {
            globalLight.color = gradients[index].Evaluate(i / time);
            yield return null;
        }
    }

    [System.Serializable]
    private struct Cycles
    {
        public string name;
        public int hourTime;
        public Color color;
        public Texture2D tex;
    }
}

