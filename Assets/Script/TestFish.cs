using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowardsPlayer : MonoBehaviour
{
    public float speed = 5f; // 移動速度
    public float continueStraightDuration = 2f; // プレイヤー位置到達後に真っ直ぐ進む時間

    private Vector3 targetPosition; // 目標位置
    private bool reachedTarget = false; // 目標位置に到達したかどうか
    private Vector3 moveDirection; // 目標位置への移動方向

    void Start()
    {
        // プレイヤーの位置を目標位置として設定
        GameObject player = GameObject.FindWithTag("Player"); // プレイヤーオブジェクトをタグで検索

        if (player != null)
        {
            targetPosition = player.transform.position;
            moveDirection = (targetPosition - transform.position).normalized; // 移動方向を計算

            transform.LookAt(new Vector3(targetPosition.x, transform.position.y, targetPosition.z));
            // 初期向きが右（X+方向）になるため、Y軸を90度回転
            transform.Rotate(0, 90, 0);

        }
        else
        {
            Debug.LogError("Playerタグを持つオブジェクトが見つかりません！");
        }
    }

    void Update()
    {
        if (!reachedTarget)
        {
            // 目標位置に向かって進む
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

            // 到達したらフラグを設定
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                reachedTarget = true;
                StartCoroutine(ContinueStraight());
            }
        }
        else
        {
            // 到達後、真っ直ぐ進む
            transform.position += moveDirection * speed * Time.deltaTime;
        }
    }

    private IEnumerator ContinueStraight()
    {
        // 指定時間後にオブジェクトを破壊
        yield return new WaitForSeconds(continueStraightDuration);
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
