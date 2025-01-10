using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectObject : MonoBehaviour
{
    public float speed = 5f; // オブジェクトの移動速度

    /*void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Shield"))
        {
            ShieldController shield = collision.gameObject.GetComponent<ShieldController>();

            if (shield != null && shield.isReflecting)
            {
                // 反射処理
                Vector3 incomingDirection = GetComponent<Rigidbody>().velocity.normalized;
                Vector3 normal = collision.contacts[0].normal; // 衝突面の法線
                Vector3 reflectDirection = Vector3.Reflect(incomingDirection, normal);

                // 新しい速度を適用
                GetComponent<Rigidbody>().velocity = reflectDirection * speed;

                // ボスの場合の追加処理
                if (CompareTag("Boss"))
                {
                    BossController boss = GetComponent<BossController>();
                    if (boss != null)
                    {
                        boss.TakeDamage(1); // ダメージを与える
                    }
                }
            }
        }
    }*/
}
