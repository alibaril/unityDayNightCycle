using UnityEngine;

public class DayNightController : MonoBehaviour {

    public Light sun;
    public float secondsInFullDay = 120f;
    [Range(0, 1)]
    public float currentTimeOfDay = 0;
    [HideInInspector]
    public float timeMultiplier = 1f;
    float sunInitialIntensity;

    static Material nightSky;
    static Material daySky;

    public GameObject meteors;
    public GameObject rain;

    void Start()
    {
        sunInitialIntensity = sun.intensity;
        daySky = RenderSettings.skybox;
        nightSky = Resources.Load("nightSky") as Material;
    }

    void Update()
    {
        UpdateSun();

        currentTimeOfDay += (Time.deltaTime / secondsInFullDay) * timeMultiplier;
        currentTimeOfDay = currentTimeOfDay % 1;

        if (currentTimeOfDay <= 0.20f || currentTimeOfDay >= 0.80f)
        {
            meteors.SetActive(true);
            rain.SetActive(false);
            RenderSettings.ambientIntensity = 3.5f;
            RenderSettings.skybox = nightSky;
        }
        else
        {
            meteors.SetActive(false);
            rain.SetActive(true);
            RenderSettings.ambientIntensity = 1.0f;
            RenderSettings.skybox = daySky;
        }
    }

    void UpdateSun()
    {
        sun.transform.localRotation = Quaternion.Euler((currentTimeOfDay * 360f) - 90, 170, 0);

        float intensityMultiplier = 1;
        if (currentTimeOfDay <= 0.23f || currentTimeOfDay >= 0.75f)
        {
            intensityMultiplier = 0;
        }
        else if (currentTimeOfDay <= 0.25f)
        {
            intensityMultiplier = Mathf.Clamp01((currentTimeOfDay - 0.23f) * (1 / 0.02f));
        }
        else if (currentTimeOfDay >= 0.73f)
        {
            intensityMultiplier = Mathf.Clamp01(1 - ((currentTimeOfDay - 0.73f) * (1 / 0.02f)));
        }

        sun.intensity = sunInitialIntensity * intensityMultiplier;
    }
}
