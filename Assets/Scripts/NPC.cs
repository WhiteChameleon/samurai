using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] GameObject player;

    private Animator anim;
    private SpriteRenderer spriteRenderer;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            anim.SetTrigger("laugh");
        }    
    }
    private void Update()
    {
        if (player.transform.position.x > transform.position.x) spriteRenderer.flipX = false;
        else if (player.transform.position.x < transform.position.x) spriteRenderer.flipX = true;
    }
}
