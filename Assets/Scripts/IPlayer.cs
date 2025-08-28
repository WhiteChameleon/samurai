using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerInput
{
    float MoveInput { get; }
    bool RunPressed { get; }
    bool JumpPressed { get; }
    void Update();
}
public interface IMovable
{
    void Move(float direction, bool run);
}
public interface IJumper
{
    void SetJumpInput(bool jumpPressed);
    void TryJump(bool move);
    bool IsGrounded();
    bool IsFall();
}
public interface IAnimator
{
    void MoveAnimations(bool run, float speed);
    void JumpAnimations(bool jump, float speed);
}
public interface IPlayerDamageable
{
    void TakeDamage(float damage);
}
public interface IPlayerHealable
{
    void Heal(float heal);
}
public interface IPlayerMovable
{
    void Move(Vector3 direction);
}
