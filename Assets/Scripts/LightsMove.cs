using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightsMove : MonoBehaviour
{
    [SerializeField] GameObject[] lights;
    [SerializeField] private float angelSpeed;
    [SerializeField] private float angelAmount;

    private float offset;

    private void Awake()
    {
        offset = Random.Range(35, 90);
    }
    private void Update()
    {
        float angel = Mathf.PingPong((Time.time + offset) * angelSpeed, angelAmount * 2) - angelAmount;
        transform.rotation = Quaternion.Euler(0, 0, angel);
    }
}
