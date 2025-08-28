using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class EndScene : MonoBehaviour
{
    [SerializeField] private float countdownDuration = 10f; // ������������ ������� � ��������
    [SerializeField] private TMP_Text countdownText; // ������������ UI-����� ��� ����������� �������
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

        Debug.Log($"�������� �������: {currentTime} ���.");
    }

    private void QuitGame()
    {
        Debug.Log("���������� ����...");
        if (exit) Application.Quit();
        else SceneManager.LoadScene("Street");
    }

    // �����������: ��� ������� � ���������
    private void OnValidate()
    {
        if (countdownDuration < 1f)
        {
            countdownDuration = 1f;
            Debug.LogWarning("������������ ������� �� ����� ���� ������ 1 �������");
        }
    }
}
