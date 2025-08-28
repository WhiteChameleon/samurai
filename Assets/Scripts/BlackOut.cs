using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BlackOut : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private SpriteRenderer blackout;
    [SerializeField] private SwitchScene trig;
    private int scene; //0 с улицы в бар, 1 с бара на улицу
    private float startScene;
    private float endScene;
    private void Awake()
    {
        startScene = GameSettings.TimeStartScene;
        endScene = GameSettings.TimeExitScene;
    }
    private void OnEnable()
    {
        trig.OnSwitchScene += ChangeScene;
    }
    private void OnDisable()
    {
        trig.OnSwitchScene -= ChangeScene;
    }
    private void Start()
    {
        player.Enable(true);
        StartCoroutine(Blackout(startScene, true));
    }
    private IEnumerator Blackout(float fadeDuration, bool clear)
    {
        float timer = 0;
        while (timer <= fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Clamp01(timer / fadeDuration);
            SetBlackAlpha(alpha, clear);
            yield return null;
        }
        if (!clear)
        {
            if (scene == 0) SceneManager.LoadScene("Bar");
            else if (scene == 1) SceneManager.LoadScene("Street");
        }
        else player.Enable(false);
    }
    private void SetBlackAlpha(float alpha, bool clear)
    {
        var spriteRenderer = blackout;
        if (spriteRenderer != null)
        {
            Color color = spriteRenderer.color;
            if (clear) color.a = 1 - alpha;
            else color.a = alpha;
            spriteRenderer.color = color;
            return;
        }
    }
    public void ChangeScene(int sceneNum)
    {
        scene = sceneNum;
        StartCoroutine(Blackout(startScene, false));
    }
}
