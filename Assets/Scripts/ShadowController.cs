using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowController : MonoBehaviour
{
    public SpriteRenderer shadowRenderer;
    public float shadowAlpha = 0.5f;

    private Material shadowMaterial;

    private void Start()
    {
        shadowMaterial = shadowRenderer.material;
        shadowMaterial.SetColor("_ShadowColor", new Color(0, 0, 0, shadowAlpha));
    }

    // Пример: изменение смещения при прыжке
    public void SetShadowOffset(float x, float y)
    {
        shadowMaterial.SetFloat("_ShadowOffsetX", x);
        shadowMaterial.SetFloat("_ShadowOffsetY", y);
    }
}
