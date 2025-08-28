using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class EndScene : MonoBehaviour
{
    [SerializeField] private float countdownDuration = 10f; // Длительность отсчета в секундах
    [SerializeField] private TMP_Text countdownText; // Опциональный UI-текст для отображения отсчета
    [SerializeField] private TMP_Text continueText;

    private float currentTime;
    private bool isCountingDown;
    private bool exit;

    private void Awake()
    {
        exit = GameSettings.ExitGame;
    }
    private void Start()
    {
        StartCountdown();
    }

    public void StartCountdown()
    {
        if (isCountingDown) return;

        currentTime = countdownDuration;
        isCountingDown = true;
        StartCoroutine(CountdownRoutine());
        StartCoroutine(ContinueRoutine());
    }

    private IEnumerator CountdownRoutine()
    {
        while (currentTime > 0)
        {
            UpdateCountdownDisplay();
            yield return new WaitForSeconds(1f);
            currentTime--;
        }

        currentTime = 0;
        UpdateCountdownDisplay();
        QuitGame();
    }
    private IEnumerator ContinueRoutine()
    {
        while (currentTime > 0)
        {
            continueText.text = ">continue<";
            yield return new WaitForSeconds(0.3f);
            continueText.text = "> continue <";
            yield return new WaitForSeconds(0.3f);
        }
    }

    private void UpdateCountdownDisplay()
    {
        if (countdownText != null)
        {
            countdownText.text = $"{Mathf.CeilToInt(currentTime)}";
        }

        Debug.Log($"Осталось времени: {currentTime} сек.");
    }

    private void QuitGame()
    {
        Debug.Log("Завершение игры...");
        if (exit) Application.Quit();
        else SceneManager.LoadScene("Street");
    }

    // Опционально: для отладки в редакторе
    private void OnValidate()
    {
        if (countdownDuration < 1f)
        {
            countdownDuration = 1f;
            Debug.LogWarning("Длительность отсчета не может быть меньше 1 секунды");
        }
    }
}
