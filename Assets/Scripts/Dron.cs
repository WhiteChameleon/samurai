using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dron : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    private float direction;
    private float cooldown;
    private float timer;
    void Start()
    {
        bool onUp = Random.value > 0.5f;
        direction = onUp ? -1 : 1;
        cooldown = Random.Range(0.5f, 5f);
    }
    void Update()
    {
        transform.position += Vector3.up * direction * moveSpeed * Time.deltaTime;
        if (timer >= cooldown)
        {
            timer = 0;
            direction = direction * -1;
        }
        else timer += Time.deltaTime;
    }
}
