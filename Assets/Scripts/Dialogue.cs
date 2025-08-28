using System.Collections;
using System;
using TMPro;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    private float textSpeed;
    [SerializeField] private TMP_Text dialogueText;

    [SerializeField] PlayerController player;
    [SerializeField] private GameObject playerBubble;
    [SerializeField] private GameObject drunkBubble;
    [SerializeField] private GameObject yakuzaBubble;
    [SerializeField] private GameObject skullBubble;

    private string[] yakuzaText = new string[] { "Опять ты, все вы к ночи \nвозвращаетесь", "..."};
    private string[] drunkText = new string[] { "Ау.. ставь меня в п.. окое", "..." };
    private string[] skullText = new string[] { "Что выпьешь? \"Энтропия\" ? \n\"Ядовитый Ками\"? ", "Саке." };
    private string[] carText = new string[] { "Ещё не время. ", "Почему она открыта?", "Выглядит знакомо." };
    private string[] replicas;

    private int dialogNumber = 0;
    private int queueDialogue = 0;
    private bool isEndTyping;
    private int typeReplicas;

    public event Action EndDialog;
    private void Awake()
    {
        playerBubble.SetActive(false);
        yakuzaBubble.SetActive(false);
        skullBubble.SetActive(false);
        textSpeed = GameSettings.TypingSpeed;
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && isEndTyping)
        {
            if (dialogNumber >= replicas.Length) 
            {
                UpdateBubble(3);
                EndDialogue();
                return;
            }
            else StartCoroutine(TypeText(replicas[dialogNumber]));
        }    
    }
    public void StartDialogue(int type)
    {
        typeReplicas = type;
        switch (typeReplicas)  // 0 якудза, 1 пьяный, 2 череп, 3 тачка
        {
            case 0:
                replicas = yakuzaText;
                break;
            case 1:
                replicas = drunkText;
                break;
            case 2:
                replicas = skullText;
                break;
            case 3:
                int rand = UnityEngine.Random.Range(0, 3);
                replicas = new string[1];
                replicas[0] = carText[rand];
                break;
        }
        if (replicas.Length == 0) return;

        StartCoroutine(TypeText(replicas[dialogNumber]));
    }
    private IEnumerator TypeText(string text)
    {
        isEndTyping = false;
        queueDialogue ^= 1;
        dialogueText.text = "";
        UpdateBubble(queueDialogue);
        foreach (char letter in text.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }
        dialogNumber += 1;
        isEndTyping = true;
    }
    private void UpdateBubble(int type)
    {
        playerBubble.SetActive(false);
        yakuzaBubble.SetActive(false);
        skullBubble.SetActive(false);
        drunkBubble.SetActive(false);
        switch (type)
        {
            case 0: // Игрок
                playerBubble.SetActive(true);
                break;
            case 1:
                switch (typeReplicas)
                {
                    case 0:
                        yakuzaBubble.SetActive(true);
                        break;
                    case 1:
                        drunkBubble.SetActive(true);
                        break;
                    case 2:
                        skullBubble.SetActive(true);
                        break;
                    case 3:
                        playerBubble.SetActive(true);
                        break;
                }
                break;
            case 3:
                break;
        }
    }
    public void EndDialogue()
    {
        if (typeReplicas != 2)
        {
            dialogNumber = 0;
            queueDialogue = 0;
            isEndTyping = false;
            dialogueText.text = "";
            player.Enable(false);
        }
        else if (typeReplicas == 2)
        {
            dialogNumber = 0;
            queueDialogue = 0;
            isEndTyping = false;
            dialogueText.text = "";
            //player.Enable(false);
            EndDialog?.Invoke();
            return;
        }
    }
}
