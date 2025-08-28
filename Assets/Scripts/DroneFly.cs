using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneFly : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float arrivalThreshold = 0.1f;

    [Header("Wobble Settings")]
    [SerializeField] private float wobbleHeight = 0.5f;
    [SerializeField] private float wobbleFrequency = 1f;
    [SerializeField] private bool useSmoothWobble = true;

    private SpriteRenderer spriteRenderer;
    private Transform currentTarget;
    private bool isFacingRight = true;
    private Vector3 originalPosition;
    private float wobbleOffset;
    private float currentWobblePhase;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        wobbleOffset = Random.Range(0f, 2f * Mathf.PI); // Для разнообразия

        if (pointA == null || pointB == null)
        {
            Debug.LogError("Points A and B must be assigned in the inspector!");
            enabled = false;
            return;
        }

        currentTarget = pointB;
    }
    private void Start()
    {
        originalPosition = pointA.position;
        transform.position = pointA.position;
    }

    private void Update()
    {
        // Сохраняем базовую позицию без wobble
        originalPosition = Vector3.MoveTowards(
            originalPosition,
            currentTarget.position,
            speed * Time.deltaTime
        );

        // Применяем движение с wobble
        transform.position = CalculateWobblePosition(originalPosition);

        // Проверка достижения цели
        if (Vector3.Distance(originalPosition, currentTarget.position) < arrivalThreshold)
        {
            currentTarget = currentTarget == pointA ? pointB : pointA;

            bool shouldFaceRight = (currentTarget == pointB);
            if (isFacingRight != shouldFaceRight)
            {
                Flip();
            }
        }
    }

    private Vector3 CalculateWobblePosition(Vector3 basePosition)
    {
        currentWobblePhase += Time.deltaTime * wobbleFrequency;
        float wobbleAmount;

        if (useSmoothWobble)
        {
            // Плавное синусоидальное покачивание
            wobbleAmount = Mathf.Sin(currentWobblePhase + wobbleOffset) * wobbleHeight;
        }
        else
        {
            // Более резкое покачивание (используем PingPong)
            wobbleAmount = Mathf.PingPong(currentWobblePhase + wobbleOffset, 2f) - 1f;
            wobbleAmount *= wobbleHeight;
        }

        return basePosition + Vector3.up * wobbleAmount;
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        spriteRenderer.flipX = !spriteRenderer.flipX;
    }

    private void OnDrawGizmos()
    {
        if (pointA != null && pointB != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(pointA.position, 0.2f);
            Gizmos.DrawSphere(pointB.position, 0.2f);
            Gizmos.DrawLine(pointA.position, pointB.position);

            // Визуализация амплитуды wobble
            if (Application.isPlaying)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawLine(originalPosition, transform.position);
            }
        }
    }
}
