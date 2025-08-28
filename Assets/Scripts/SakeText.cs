using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SakeText : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 1f; // Скорость движения вверх
    public float maxHeight = 2f; // Максимальная высота перед исчезновением

    [Header("Fade Settings")]
    public float fadeDuration = 1f; // Время за которое объект полностью исчезнет
    private float currentFadeTime = 0f;

    private SpriteRenderer spriteRenderer;
    private Vector3 initialPosition;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogWarning("No SpriteRenderer found on object. Fading will not work.");
        }

        initialPosition = transform.position;
    }

    private void Update()
    {
        // Движение вверх
        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);

        // Увеличение времени затухания
        currentFadeTime += Time.deltaTime;

        // Изменение прозрачности
        if (spriteRenderer != null)
        {
            float alpha = Mathf.Lerp(1f, 0f, currentFadeTime / fadeDuration);
            spriteRenderer.color = new Color(
                spriteRenderer.color.r,
                spriteRenderer.color.g,
                spriteRenderer.color.b,
                alpha
            );
        }

        // Уничтожение объекта после достижения максимальной высоты или полного исчезновения
        if (transform.position.y > initialPosition.y + maxHeight ||
            (spriteRenderer != null && spriteRenderer.color.a <= 0))
        {
            Destroy(gameObject);
        }
    }
}
