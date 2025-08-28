using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScene : MonoBehaviour
{
    [SerializeField] PlayerController player;
    [SerializeField] GameObject bttnE;

    public event Action<int> OnSwitchScene;
    private bool inputE;

    private void Start()
    {
        bttnE.SetActive(false);
    }
    private void Update()
    {
        inputE = Input.GetKey(KeyCode.E);
    }
    public void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            bttnE.SetActive(true);
            if (inputE)
            {
                player.Enable(true);
                Scene currentScene = SceneManager.GetActiveScene();
                if (currentScene.name == "Street")
                {
                    SpawnPlayer.Instance.RegisterSceneTransition("Street");
                    OnSwitchScene?.Invoke(0);
                }
                else if (currentScene.name == "Bar")
                {
                    SpawnPlayer.Instance.RegisterSceneTransition("Bar");
                    OnSwitchScene?.Invoke(1); 
                }
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        bttnE.SetActive(false);
    }
}
