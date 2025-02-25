using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnimationController : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator が見つかりません！ボスに Animator コンポーネントが必要です。");
        }
    }

    // 突進アニメーション（ON/OFF）
    public void SetChargeState(bool isCharging)
    {
        if (animator != null)
        {
            animator.SetBool("isCharging", isCharging);
            Debug.Log($"ボスの突進アニメーション: {(isCharging ? "開始" : "終了")}");
        }
    }

    // 後退アニメーション（ON/OFF）
    public void SetRetreatState(bool isRetreating)
    {
        if (animator != null)
        {
            animator.SetBool("isRetreating", isRetreating);
            Debug.Log($"ボスの後退アニメーション: {(isRetreating ? "開始" : "終了")}");
        }
    }

    // 攻撃アニメーション（ON/OFF）
    public void SetAttackState(bool isAttacking)
    {
        if (animator != null)
        {
            animator.SetBool("isAttacking", isAttacking);
            Debug.Log($"ボスの攻撃アニメーション: {(isAttacking ? "開始" : "終了")}");
        }
    }
}
