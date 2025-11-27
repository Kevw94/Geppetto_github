using UnityEngine;

public class FlickerLight : MonoBehaviour
{
    public Light flickerLight;
    public float minIntensity = 0.0f;
    public float maxIntensity = 1.2f;
    public float flickerSpeed = 0.1f;
    [Range(0f, 1f)] public float blackoutChance = 0.1f;

    private float targetIntensity;
    private float timer;

    void Start()
    {
        if (flickerLight == null) flickerLight = GetComponent<Light>();
        targetIntensity = flickerLight.intensity;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            bool blackout = Random.value < blackoutChance;

            // Random blackout or intensity
            targetIntensity = blackout ? 0f : Random.Range(minIntensity, maxIntensity);
            timer = Random.Range(flickerSpeed * 0.5f, flickerSpeed * 1.5f);
        }

        flickerLight.intensity = Mathf.Lerp(flickerLight.intensity, targetIntensity, Time.deltaTime * 5f);
    }
}
