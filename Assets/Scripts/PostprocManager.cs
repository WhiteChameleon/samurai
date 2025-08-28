using URPGlitch.Runtime.AnalogGlitch;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using URPGlitch.Runtime.DigitalGlitch;
using System;
using UnityEngine.SceneManagement;

public class PostprocManager : MonoBehaviour
{
    [SerializeField] GameObject imageBottle;
    [SerializeField] GameObject imageUI;
    //0.5f
    // 0.1f
    // 0.005f
    // 0.3f
    // 0.5f
    /*private float target1 = 0.5f;
    private float target2 = 0.1f;
    private float target3 = 0.005f;
    private float target4 = 0.3f;
    private float target5 = 0.5f;*/
    private float target1 = 0.1f;
    private float target2 = 0.02f;
    private float target3 = 0.001f;
    private float target4 = 0.06f;
    private float target5 = 0.1f;

    [Header("Lerp Settings")]
    private AnimationCurve lerpCurve = AnimationCurve.Linear(0, 0, 1, 1);

    [Header("Scene Settings")]
    private string nextSceneName = "Final";
    private float sceneLoadDelay = 1f;

    private float current1;
    private float current2;
    private float current3;
    private float current4;
    private float current5;

    [SerializeField] private Volume _volume;
    private AnalogGlitchVolume _analogGlitch;
    private DigitalGlitchVolume _digitalGlitch;

    [SerializeField] private Dialogue dialogue;
    public bool isChange { get; private set; }


    private float lerpDuration;
    private float powerGlitch;
    private void OnEnable()
    {
        if(dialogue != null) dialogue.EndDialog += StartGlitch;
    }

    private void OnDisable()
    {
        if (dialogue != null) dialogue.EndDialog -= StartGlitch;
    }
    private void Awake()
    {
        lerpDuration = GameSettings.TimeBarman;
        powerGlitch = GameSettings.PowerGlitch;
        target1 = target1 * powerGlitch;
        target2 = target2 * powerGlitch;
        target3 = target3 * powerGlitch;
        target4 = target4 * powerGlitch;
        target5 = target5 * powerGlitch;
    }
    private void Start()
    {
        isChange = false;
        if (_volume.profile.TryGet(out _analogGlitch))
        {
            _analogGlitch.active = true;
        }
        if (_volume.profile.TryGet(out _digitalGlitch))
        {
            _digitalGlitch.active = true;
        }
        Scene currentScene = SceneManager.GetActiveScene();

        // Проверяем по имени
        if (currentScene.name == "Final")
        {
            EndGlitch();
        }
        if (imageBottle != null)
        {
            imageBottle.SetActive(false);
        }
    }
    public void SetGlitch(
        float scanLineJitter,
        float verticalJump,
        float horizontalShake,
        float colorDrift,
        float intensity
    )
    {
        _analogGlitch.scanLineJitter.value = scanLineJitter;
        _analogGlitch.verticalJump.value = verticalJump;
        _analogGlitch.horizontalShake.value = horizontalShake;
        _analogGlitch.colorDrift.value = colorDrift;

        _digitalGlitch.intensity.value = intensity;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (isChange)
            {
                return;
            }
            SetGlitch(target1, target2, target3, target4, target5);
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            if (isChange) 
            {
                isChange = true;
                StopAllCoroutines();
            }
            SetGlitch(0f, 0f, 0f, 0f, 0f);
        }
    }
    public void StartGlitch()
    {
        StartCoroutine(LerpAndLoadScene());
    }
    private IEnumerator LerpAndLoadScene()
    {
        if(imageBottle != null)
        {
            imageBottle.SetActive(true);
            imageUI.SetActive(false);
        }
        yield return new WaitForSeconds(1f);
        isChange = true;
        float elapsedTime = 0f;
        current1 = current2 = current3 = current4 = current5 = 0f;

        while (elapsedTime < lerpDuration)
        {
            float t = lerpCurve.Evaluate(elapsedTime / lerpDuration);

            current1 = Mathf.Lerp(0f, target1, t);
            current2 = Mathf.Lerp(0f, target2, t);
            current3 = Mathf.Lerp(0f, target3, t);
            current4 = Mathf.Lerp(0f, target4, t);
            current5 = Mathf.Lerp(0f, target5, t);

            SetGlitch(current1, current2, current3, current4, current5);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Гарантируем точные конечные значения
        current1 = target1;
        current2 = target2;
        current3 = target3;
        current4 = target4;
        current5 = target5;

        isChange = false;
        // Задержка и загрузка сцены
        yield return new WaitForSeconds(sceneLoadDelay);
        SceneManager.LoadScene(nextSceneName);
    }
    public void EndGlitch()
    {
        StartCoroutine(LerpAndExit());
    }
    private IEnumerator LerpAndExit()
    {
        isChange = true;
        float elapsedTime = 0f;
        float duration = lerpDuration / 2;
        current1 = target1;
        current2 = target2;
        current3 = target3;
        current4 = target4;
        current5 = target5;

        while (elapsedTime < duration)
        {
            float t = lerpCurve.Evaluate(elapsedTime / duration);

            current1 = Mathf.Lerp(target1, 0, t);
            current2 = Mathf.Lerp(target2, 0, t);
            current3 = Mathf.Lerp(target3, 0, t);
            current4 = Mathf.Lerp(target4, 0, t);
            current5 = Mathf.Lerp(target5, 0, t);

            SetGlitch(current1, current2, current3, current4, current5);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        current1 = 0;
        current2 = 0;
        current3 = 0;
        current4 = 0;
        current5 = 0;

        isChange = false;
        yield return new WaitForSeconds(sceneLoadDelay);
    }
}
