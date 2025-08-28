using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightsDynamic : MonoBehaviour
{
    [SerializeField] private float baseOuterRadius = 12f;
    [SerializeField] private float baseInnerRadius = 5.5f;
    [SerializeField] private float radiusPulseAmount = 0.5f;
    [SerializeField] private float radiusPulseSpeed = 1f;

    [SerializeField] private float baseIntensity = 1f;
    [SerializeField] private float intensityPulseAmount = 0.2f;
    [SerializeField] private float intensityPulseSpeed = 1.5f;

    private Light2D spotLight;
    private float pulsePhase;

    private void Awake()
    {
        spotLight = GetComponent<Light2D>();

        spotLight.pointLightOuterRadius = baseOuterRadius;
        spotLight.pointLightInnerRadius = baseInnerRadius;
        spotLight.intensity = baseIntensity;

        pulsePhase = Random.Range(0f, Mathf.PI * 2);
    }

    private void Update()
    {
        pulsePhase += Time.deltaTime * radiusPulseSpeed;
        float pulseFactor = (Mathf.Sin(pulsePhase) + 1f) * 0.5f;
        spotLight.pointLightOuterRadius = baseOuterRadius + pulseFactor * radiusPulseAmount;
        spotLight.pointLightInnerRadius = baseInnerRadius + pulseFactor * radiusPulseAmount * 0.5f;

        float intensityPulse = Mathf.Sin(Time.time * intensityPulseSpeed) * intensityPulseAmount;
        spotLight.intensity = baseIntensity + intensityPulse;
    }
}
