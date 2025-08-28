using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    private Vector3 lastCameraTransform;

    [SerializeField] private Transform[] houses;
    [SerializeField] private float parallaxSpeed = 0.5f;

    [SerializeField] private Transform[] backHouses;
    [SerializeField] private Transform[] middleHouses;
    [SerializeField] private Transform[] frontHouses;
    [SerializeField] private Transform[] bg;
    [SerializeField] private float tileWidth = 10f;
    private void Start()
    {
        lastCameraTransform = cameraTransform.position;
    }
    private void Update()
    {
        ParallaxMove(frontHouses);
        ParallaxMove(middleHouses);
        ParallaxMove(backHouses);
        ParallaxMove(bg);
    }
    private void LateUpdate()
    {
        Vector3 move = cameraTransform.position - lastCameraTransform;
        for (int i = 0; i < houses.Length; i++)
        {
            houses[i].position += new Vector3(move.x * parallaxSpeed * (1 + i), 0, 0);
        }
        lastCameraTransform = cameraTransform.position;
    }
    private void ParallaxMove(Transform[] tiles)
    {
        foreach (var tile in tiles)
        {
            if (tile.position.x < cameraTransform.position.x - 15f)
            {
                tile.position += Vector3.right * (tileWidth * tiles.Length);
            }
            else if (tile.position.x > cameraTransform.position.x + 15f)
            {
                tile.position -= Vector3.right * (tileWidth * tiles.Length);
            }
        }
    }
}
