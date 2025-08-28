using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingsMenuUI : MonoBehaviour
{
    [Header("Speed Settings")]
    public Slider typingSpeedSlider;

    [Header("Time Settings")]
    public Slider timeBarmanSlider;
    public Slider timeExitSceneSlider;
    public Slider timeStartSceneSlider;

    [Header("Level Settings")]
    public Slider powerGlitchSlider;
    public Toggle exitGameToggle;

    [Header("Buttons")]
    public Button saveButton;
    public Button resetButton;
    public Button startButton;

    private void Start()
    {
        // Инициализация значений слайдеров
        typingSpeedSlider.value = GameSettings.TypingSpeed;

        timeBarmanSlider.value = GameSettings.TimeBarman;
        timeExitSceneSlider.value = GameSettings.TimeExitScene;
        timeStartSceneSlider.value = GameSettings.TimeStartScene;

        powerGlitchSlider.value = GameSettings.PowerGlitch;
        exitGameToggle.isOn = GameSettings.ExitGame;

        // Назначение обработчиков событий
        saveButton.onClick.AddListener(SaveSettings);
        resetButton.onClick.AddListener(ResetToDefault);
        startButton.onClick.AddListener(StartGame);

        // Добавление обработчиков
        exitGameToggle.onValueChanged.AddListener(OnExitToggleChanged);
    }

    private void SaveSettings()
    {
        // Сохранение значений из слайдеров в GameSettings
        GameSettings.TypingSpeed = typingSpeedSlider.value;

        GameSettings.TimeBarman = (int)timeBarmanSlider.value;
        GameSettings.TimeExitScene = (int)timeExitSceneSlider.value;
        GameSettings.TimeStartScene = (int)timeStartSceneSlider.value;

        GameSettings.PowerGlitch = (int)powerGlitchSlider.value;

        Debug.Log("Settings saved!");
    }

    private void OnExitToggleChanged(bool isOn)
    {
        GameSettings.ExitGame = isOn;
    }
    private void ResetToDefault()
    {
        // Сброс к значениям по умолчанию (можете изменить эти значения)
        typingSpeedSlider.value = 0.1f;

        timeBarmanSlider.value = 5;
        timeExitSceneSlider.value = 1;
        timeStartSceneSlider.value = 1;

        powerGlitchSlider.value = 1f;
        OnExitToggleChanged(true);
        exitGameToggle.isOn = GameSettings.ExitGame;
        SaveSettings();
    }
    private void StartGame()
    {
        SaveSettings();
        SceneManager.LoadScene("Street");
    }
}
