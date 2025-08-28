using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    [SerializeField] private GameObject carPrefab;
    [SerializeField] private Camera mainCamera;

    [SerializeField] private float minCooldown = 5f;
    [SerializeField] private float maxCooldown = 15f;

    private float destroyDistance = 35f;
    void Start()
    {
        StartCoroutine(SpawnCar());
    }
    private IEnumerator SpawnCar()
    {
        while (true)
        {
            float waitTime = Random.Range(minCooldown, maxCooldown);
            yield return new WaitForSeconds(waitTime);
            InstCar();
        }
    }
    private void InstCar()
    {
        bool spawnOnLeft = Random.value > 0.5f;
        float xOffset = (mainCamera.ViewportToWorldPoint(new Vector3(1f, 0, 0))).x;
        xOffset = spawnOnLeft ? xOffset * -1 : xOffset * 1;
        float yOffset = Random.Range(mainCamera.ViewportToWorldPoint(new Vector3(0, 0.8f, 0)).y, mainCamera.ViewportToWorldPoint(new Vector3(0, 1f, 0)).y);
        Vector3 spawnPos = new Vector3(xOffset, yOffset, 0);
        Debug.Log($"Позиция спавна {spawnPos}");
        GameObject car = Instantiate(carPrefab, spawnPos, Quaternion.identity);

        Car carScript = car.GetComponent<Car>();

        carScript.moveSpeed = Random.Range(1f, 3f);
        carScript.direction = spawnOnLeft ? 1f : -1f;

        StartCoroutine(DestroyCar(car));
    }
    private IEnumerator DestroyCar(GameObject car)
    {
        while (car != null)
        {
            float distCamera = Vector3.Distance(car.transform.position, mainCamera.transform.position);
            if(distCamera > destroyDistance)
            {
                Destroy(car);
            }
            yield return null;
        }
    }
}
