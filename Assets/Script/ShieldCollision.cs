using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldCollision : MonoBehaviour
{
    public ShieldController shieldController; // ShieldControllerへの参照

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"トリガーに入ったオブジェクト: {other.gameObject.name}");

        // Fish または Rubble タグのオブジェクトと接触した場合
        if (other.gameObject.CompareTag("Fish") || other.gameObject.CompareTag("Rubble"))
        {
            shieldController?.ReduceShieldHP(); // HPを減少させる
            Debug.Log($"Shieldが {other.gameObject.tag} とトリガーで接触しました");
        }
        else if (other.gameObject.CompareTag("Boss"))
        {
            if (!shieldController.IsReflecting()) // 反射中でない場合のみ
            {
                Debug.Log("ボスがシールドに衝突！");
                shieldController.RegisterBossShieldCollision(); // シールドがボスと衝突したことを記録
                shieldController.ApplyBossDamage(); // HP -5
                shieldController.ApplyKnockbackToPlayer(other.transform.position); // **プレイヤーをノックバック**
            }
            else
            {
                Debug.Log("反射モード中のため、Boss の攻撃を無効化！");
            }
        }
        else
        {
            Debug.Log($"対応しないオブジェクトがトリガーに入りました: {other.gameObject.tag}");
        }
    }
}
