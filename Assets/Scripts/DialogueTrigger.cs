using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private int typeDialogue;
    [SerializeField] private Dialogue script;
    [SerializeField] PlayerController player;
    [SerializeField] GameObject bttnE;

    private bool inputE;
    private bool dialogactive;
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
        if (other.gameObject.CompareTag("Player") && !dialogactive)
        {
            bttnE.SetActive(true);
            if (inputE)
            {
                player.Enable(true);
                script.StartDialogue(typeDialogue);
                dialogactive = true;
                bttnE.SetActive(false);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        dialogactive = false;
        bttnE.SetActive(false);
    }
}
