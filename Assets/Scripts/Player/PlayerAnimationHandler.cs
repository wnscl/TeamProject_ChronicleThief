using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationHandler : MonoBehaviour
{
    protected Animator animator;


    private static readonly int MoveHash = Animator.StringToHash("IsMove");
    private static readonly int DamageHash = Animator.StringToHash("IsDamage");
    private static readonly int DeathHash = Animator.StringToHash("IsDead");
    private static readonly int BlinkHash = Animator.StringToHash("IsBlink");

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void SetMovement(bool isMoving) // �̵� anim (Idle -> Move)
    {
        animator.SetBool(MoveHash, isMoving);
    }

    public void PlayDamage()
    {
        animator.SetBool(DamageHash, true);
    }

    public void InvincibilityEnd()
    {
        animator.SetBool(DamageHash, false);
    }

    public void PlayDeath()
    {
        animator.SetBool(DeathHash, true);
    }

    //public void PlayBlinkStart()
    //{
    //    animator.SetBool(BlinkHash, true);
    //}

    //public void PlayBlinkEnd()
    //{
    //    animator.SetBool(BlinkHash, false);
    //}

    public void SetBlink(bool state)
    {
        animator.SetBool("IsBlink", state);

        // �߿�! ��ġ/ȸ�� ���� ����
        animator.applyRootMotion = false;
    }
}
