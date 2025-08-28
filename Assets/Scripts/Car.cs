using UnityEngine;

public class Car : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float direction;
    private float timer;
    private Vector3 pos;
    private void Start()
    {
        float scale = Random.Range(0.5f, 1f);
        transform.localScale = new Vector3(scale * direction, scale, transform.localScale.y);
        pos = transform.position;
    }
    private void Update()
    {
        transform.position += Vector3.right * direction * moveSpeed * Time.deltaTime;
        /*float swayY = (Time.time * swaySpeed) * swayAmount;
        transform.position = new Vector3(transform.position.x, swayY, transform.position.z);
        timer += Time.deltaTime;
        if (timer >= directionTime)
        {
            direction *= -1;
            timer = 0;
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }*/
    }
}
