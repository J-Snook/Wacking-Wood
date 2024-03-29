using System.Collections;
using UnityEngine;

public class DayNightCycle : MonoBehaviour, IDataPersistance
{
    [SerializeField] private Cycles[] cycles;
    [SerializeField] private float numSecondsPerMinute = 1f;
    [SerializeField] private float deltaTimeMultipler = 1f;
    [SerializeField] private Light globalLight;
    [SerializeField] private PlayerUI _playerUI;
    [HideInInspector] public float staminaRegenReduction;

    #region Singleton
    public static DayNightCycle instance;
    #endregion

    private int minutes;
    public int Minutes { get { return minutes; } set { minutes = value;OnMinuteChange(value); } }
    
    private int hours;
    public int Hours { get { return hours; } set { hours = value; OnHourChange(value); } }
    
    private int days;
    public int Days { get { return days; } set {days=value; } }

    private float tempSeconds;
    private Gradient[] gradients;

    private void Awake()
    {
        instance= this;
    }

    private void Start()
    {
        gradients = new Gradient[cycles.Length];
        GradientAlphaKey[] alphas = new GradientAlphaKey[2] { new GradientAlphaKey(1f, 1f), new GradientAlphaKey(1f, 1f) };
        for(int i = 0; i < cycles.Length; i++)
        {
            gradients[i] = new Gradient();
            GradientColorKey[] colourKey = new GradientColorKey[2] { new GradientColorKey(cycles[(i-1<0) ? cycles.Length-1 :i-1 ].color, 0f), new GradientColorKey(cycles[i].color, 10f) };
            gradients[i].SetKeys(colourKey, alphas);
        }
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
                float transitionTime = (Mathf.Abs(cycles[(i + 1 > cycles.Length) ? 0 : i+1].hourTime - cycles[i].hourTime)/4f)*60f;
                StartCoroutine(LerpSkybox(cycles[(i - 1 < 0) ? cycles.Length-1 : i-1].tex, cycles[i].tex, transitionTime));
                StartCoroutine(LerpLight(i, transitionTime));
                StartCoroutine(LerpFogDensity(cycles[(i - 1 < 0) ? cycles.Length - 1 : i - 1].fogDensity, cycles[i].fogDensity, transitionTime));
                staminaRegenReduction = cycles[i].reduceStamina;
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
            Color colour = gradients[index].Evaluate(i / time);
            globalLight.color = colour;
            RenderSettings.fogColor = colour;
            yield return null;
        }
    }

    private IEnumerator LerpFogDensity(float from,float to, float time)
    {
        for(float i = 0; i < time; i+=Time.deltaTime)
        {
            RenderSettings.fogDensity = Mathf.Lerp(from, to, i / time);
            yield return null;
        }
    }

    public void LoadData(GameData data)
    {
        minutes= data.minutes;
        hours= data.hours;
        days= data.days;
        int index=-1;
        if ( hours < cycles[1].hourTime || hours >= cycles[0].hourTime)
        {
            index = 0;
        }
        else
        {
            for(int i = 1; i < cycles.Length; i++)
            {
                if(hours >= cycles[i].hourTime && hours < cycles[(i+1>cycles.Length-1) ? 0 : i+1].hourTime)
                {
                    index = i;
                }
            }
        }
        RenderSettings.skybox.SetTexture("_Texture1", cycles[index].tex);
        RenderSettings.skybox.SetTexture("_Texture2", cycles[index].tex);
        RenderSettings.skybox.SetFloat("_Blend", 0);
        globalLight.color = cycles[index].color;
        RenderSettings.fogColor = cycles[index].color;
        RenderSettings.fogDensity = cycles[index].fogDensity;
        staminaRegenReduction = cycles[index].reduceStamina;
    }

    public void SaveData(ref GameData data)
    {
        data.minutes = minutes;
        data.hours= hours;
        data.days= days;
    }

    [System.Serializable]
    private struct Cycles
    {
        public string name;
        public int hourTime;
        public float fogDensity;
        public Color color;
        public Texture2D tex;
        public float reduceStamina;
    }
}

