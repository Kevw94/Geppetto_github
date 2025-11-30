using UnityEngine;

public class FlickerLight : MonoBehaviour
{
    public Light flickerLight;
    public MeshRenderer meshRenderer;

    public float flickerSpeed = 0.1f;
    [Range(0f, 1f)] public float blackoutChance = 0.3f;

    private float timer;

    void Start()
    {
        if (flickerLight == null)
            flickerLight = GetComponent<Light>();

        if (meshRenderer == null)
            meshRenderer = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            bool blackout = Random.value < blackoutChance;

            flickerLight.enabled = !blackout;

            if (meshRenderer != null)
                meshRenderer.enabled = !blackout;

            timer = Random.Range(flickerSpeed * 0.5f, flickerSpeed * 1.5f);
        }
    }
}
