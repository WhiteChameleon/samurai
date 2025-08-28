using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlitchController : MonoBehaviour
{
    [Range(0, 1)] public float glitchIntensity = 0f;
    [Range(0.1f, 10f)] public float glitchSpeed = 1f;

    private Material glitchMaterial;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        glitchMaterial = spriteRenderer.material;
    }

    void Update()
    {
        if (glitchMaterial != null)
        {
            glitchMaterial.SetFloat("_GlitchAmount", glitchIntensity);
            glitchMaterial.SetFloat("_GlitchSpeed", glitchSpeed);

            // Автоматическое мерцание (опционально)
            if (Random.value > 0.95f)
            {
                glitchIntensity = Random.Range(0.1f, 0.5f);
            }
            else if (glitchIntensity > 0)
            {
                glitchIntensity = Mathf.Max(0, glitchIntensity - Time.deltaTime * 0.5f);
            }
        }
    }

    // Метод для внешнего управления глитчем
    public void TriggerGlitch(float intensity, float duration)
    {
        StartCoroutine(DoGlitch(intensity, duration));
    }

    private IEnumerator DoGlitch(float intensity, float duration)
    {
        float originalIntensity = glitchIntensity;
        glitchIntensity = intensity;

        yield return new WaitForSeconds(duration);

        glitchIntensity = originalIntensity;
    }
}
