using UnityEngine;

[CreateAssetMenu(fileName = "CameraData", menuName = "Data/CameraData")]
public class CameraData : ScriptableObject
{
    [Header("��������� ��������")]
    public Vector2 targetOffset = new Vector2(0f, 1f);

    [Header("��������� ��������")]
    public float smoothTime = 0.2f;
    public float maxSpeed = 15f;

    [Header("��������� ����������")]
    public float lookAheadFactor = 0.5f;
    public float lookAheadReturnSpeed = 2f;
    public float lookAheadMoveThreshold = 0.1f;
}
