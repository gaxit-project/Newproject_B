using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseFish : MonoBehaviour
{
    public float speed = 6f; // 移動速度

    private Vector3 targetPosition; // 目標位置
    private Vector3 moveDirection; // 目標位置への移動方向

    void Start()
    {
        Invoke("Dissappear", 7); // 7秒後にオブジェクトを破壊
    }

    void Update()
    {
        Invoke("DefinePlayerPosition", 0); // 目標位置を更新
        // 目標位置に向かって進む
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

    }

    private void DefinePlayerPosition()
    {
        // プレイヤーの位置を目標位置として設定
        GameObject player = GameObject.FindWithTag("Player"); // プレイヤーオブジェクトをタグで検索

        if (player != null)
        {
            targetPosition = player.transform.position;
            moveDirection = (targetPosition - transform.position); // 移動方向を計算
        }
        else
        {
            Debug.LogError("Playerタグを持つオブジェクトが見つかりません！");
        }
    }

    private void Dissappear() //オブジェクトを破壊
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        // ShieldタグまたはPlayerタグと衝突した場合にオブジェクトを破壊
        if (other.CompareTag("Shield") || other.CompareTag("Player"))
        {
            Debug.Log($"{gameObject.name} が {other.gameObject.tag} と衝突し破壊されました。");
            Destroy(gameObject);
        }
    }
}
