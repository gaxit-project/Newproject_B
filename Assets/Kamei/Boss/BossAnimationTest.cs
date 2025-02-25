using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnimationTest : MonoBehaviour
{
    private BossAnimationController bossAnimation;
    private bool isCharging = false;
    private bool isRetreating = false;
    private bool isAttacking = false;

    void Start()
    {
        bossAnimation = GetComponent<BossAnimationController>();
        if (bossAnimation == null)
        {
            Debug.LogError("BossAnimationController が見つかりません！このスクリプトをボスにアタッチしてください。");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I)) // 突進のON/OFF
        {
            isCharging = !isCharging;
            bossAnimation.SetChargeState(isCharging);
        }

        if (Input.GetKeyDown(KeyCode.O)) // 後退のON/OFF
        {
            isRetreating = !isRetreating;
            bossAnimation.SetRetreatState(isRetreating);
        }

        if (Input.GetKeyDown(KeyCode.P)) // 攻撃のON/OFF
        {
            isAttacking = !isAttacking;
            bossAnimation.SetAttackState(isAttacking);
        }
    }
}
