using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class GlitchCamera : MonoBehaviour
{
    [Range(0, 1)] public float intensity = 0f;
    [Range(0, 0.1f)] public float colorShift = 0.02f;
    [Range(0, 1f)] public float scanLineJitter = 0.1f;
    [Range(0, 1f)] public float verticalJump = 0.1f;
    [Range(0, 1f)] public float horizontalShake = 0.1f;
    public Texture2D displacementMap;

    public Shader glitchShader;
    private Material glitchMaterial;

    void OnEnable()
    {
        if (glitchShader == null)
            glitchShader = Shader.Find("Hidden/GlitchPostEffect");

        glitchMaterial = new Material(glitchShader);
        glitchMaterial.hideFlags = HideFlags.HideAndDontSave;
    }

    void OnDisable()
    {
        if (glitchMaterial != null)
            DestroyImmediate(glitchMaterial);
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (glitchMaterial == null || intensity <= 0f)
        {
            Graphics.Blit(source, destination);
            return;
        }

        // Установка параметров
        glitchMaterial.SetFloat("_Intensity", intensity);
        glitchMaterial.SetFloat("_ColorIntensity", colorShift);
        glitchMaterial.SetFloat("_ScanLineJitter", scanLineJitter);
        glitchMaterial.SetFloat("_VerticalJump", verticalJump);
        glitchMaterial.SetFloat("_HorizontalShake", horizontalShake);
        glitchMaterial.SetTexture("_DisplacementMap", displacementMap);

        // Случайные флуктуации для более органичного эффекта
        glitchMaterial.SetFloat("_RandomValue", Random.value);

        Graphics.Blit(source, destination, glitchMaterial);
    }

    // Метод для триггера глитч-эффекта
    public void TriggerGlitch(float duration, float maxIntensity = 0.7f)
    {
        StartCoroutine(GlitchRoutine(duration, maxIntensity));
    }

    private IEnumerator GlitchRoutine(float duration, float maxIntensity)
    {
        float timer = 0f;
        float originalIntensity = intensity;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            intensity = Mathf.Lerp(originalIntensity, maxIntensity, timer / duration * 2f);

            if (timer > duration * 0.5f)
            {
                intensity = Mathf.Lerp(maxIntensity, originalIntensity, (timer - duration * 0.5f) / (duration * 0.5f));
            }

            yield return null;
        }

        intensity = originalIntensity;
    }
}
