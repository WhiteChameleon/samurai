using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Data/Player Data")]
public class PlayerData : ScriptableObject
{
    [Header("������������")]
    public float walkSpeed;
    public float runSpeed;
    public float moveAcceleration;

    [Header("������")]
    public float jumpCrouchDuration = 0.2f;
    public float jumpSpeed;
    public float jumpCoyoteTime;
    public float jumpBufferTime;
    public float groundCheckRadius;
    public LayerMask groundLayer;

    [Header("�����")]
    public float wallCheckDistance = 0.1f; // ���������� ��� �������� �����
    public LayerMask wallLayer;
}
