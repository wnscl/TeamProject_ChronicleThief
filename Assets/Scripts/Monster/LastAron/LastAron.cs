using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastAron : Aron, IBattleEntity
{

    protected override void Start()
    {
        base.Start();
    }

    protected override void AronFirstSetting()
    {
        name = "마지막아론";
        hp = 4000;
        atk = 200;
        survive = true;
        isAttacked = false;
        isSpawn = false;
        moveSpeed = 7;
        attackRange = 2f;
    }

    protected override IEnumerator AronAttack()
    {
        int chance = Random.Range(0, 10);
        float aniTimer = 0f;
        float skillTimer = 0f;
        string attackName = CheckAttackPattern(chance);

        StartAttackAnim(attackName);
        yield return new WaitForSeconds(0.005f);

        while (aniTimer < 1)
        {
            if (!survive)
            {
                yield break;
            }
            aniTimer += Time.deltaTime;
            rigid.velocity = Vector3.zero;
            yield return null;
        }

        if (SelectPrefabs == 1) Instantiate(fallingSpearSkillPrefab, playerPos, Quaternion.identity, this.transform);
        else if (SelectPrefabs == 2) Instantiate(HeartAttackSkillPrefab, playerPos, Quaternion.identity, this.transform);
        else if (SelectPrefabs == 3) Instantiate(attackPrefabs3, playerPos, Quaternion.identity, this.transform);

        while (skillTimer < attackDuration)
        {
            if (!survive)
            {
                yield break;
            }
            skillTimer += Time.deltaTime;
            rigid.velocity = Vector3.zero;
            yield return null;
        }

        attackDuration = 0f;
        canAttack = false;
        yield break;
    }

    protected override string CheckAttackPattern(int choice)
    {
        if (choice < 3)
        {
            attackDuration = 5f;
            return "FallingSpear";
        }
        else if (choice >= 3 && choice < 6)
        {
            attackDuration = 5f;
            return "HeartAttack";
        }
        else
        {
            attackDuration = 5f;
            return "AreaSkill";
        }
    }

    protected override void StartAttackAnim(string attackName)
    {
        switch (attackName)
        {
            case "FallingSpear":
                anim.SetBool("AnyAnimEnd", false);
                weaponAnim.SetBool("AnyAnimEnd", false);
                anim.SetInteger("AttackNum", 1);
                weaponAnim.SetInteger("AttackNum", 1);
                SelectPrefabs = 1;
                break;

            case "HeartAttack":
                anim.SetBool("AnyAnimEnd", false);
                weaponAnim.SetBool("AnyAnimEnd", false);
                anim.SetInteger("AttackNum", 2);
                weaponAnim.SetInteger("AttackNum", 2);
                SelectPrefabs = 2;
                break;
            case "AreaSkill":
                anim.SetBool("AnyAnimEnd", false);
                weaponAnim.SetBool("AnyAnimEnd", false);
                anim.SetInteger("AttackNum", 3);
                weaponAnim.SetInteger("AttackNum", 3);
                SelectPrefabs = 3;
                break;
            case "Move":
                anim.SetBool("AnyAnimEnd", false);
                weaponAnim.SetBool("AnyAnimEnd", false);
                anim.SetInteger("AttackNum", 10);
                weaponAnim.SetInteger("AttackNum", 10);
                break;
        }
    }

    protected override IEnumerator AronDead()
    {
        anim.SetBool("AnyAnimEnd", true);
        weaponAnim.SetInteger("AttackNum", 0);
        weaponAnim.SetBool("AnyAnimEnd", true);
        yield return new WaitForSeconds(0.005f);
        anim.SetBool("AnyAnimEnd", false);
        weaponAnim.SetBool("AnyAnimEnd", false);
        yield return new WaitForSeconds(0.005f);

        anim.SetBool("isDead", true);
        weaponAnim.SetBool("isDead", true);
        yield return new WaitForSeconds(1f);

        Debug.Log("최종 보스 사망!");

        if (BattleSystemManager.Instance != null && BattleSystemManager.Instance.waveCount == 20)
        {
            BattleSystemManager.Instance.isGameOver = true;
        }

        Destroy(this.gameObject);
        DeathPanelController.Instance.Show();
    } 
}
