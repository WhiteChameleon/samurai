using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SakeText : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 1f; // �������� �������� �����
    public float maxHeight = 2f; // ������������ ������ ����� �������������

    [Header("Fade Settings")]
    public float fadeDuration = 1f; // ����� �� ������� ������ ��������� ��������
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
        // �������� �����
        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);

        // ���������� ������� ���������
        currentFadeTime += Time.deltaTime;

        // ��������� ������������
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

        // ����������� ������� ����� ���������� ������������ ������ ��� ������� ������������
        if (transform.position.y > initialPosition.y + maxHeight ||
            (spriteRenderer != null && spriteRenderer.color.a <= 0))
        {
            Destroy(gameObject);
        }
    }
}
