using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRubble : MonoBehaviour
{
    public ShieldController shieldController; // ShieldManagerにアタッチされたShieldControllerを参照

    public float speed = 5f; // 移動速度
    public float continueStraightDuration = 2f; // プレイヤー位置到達後に真っ直ぐ進む時間

    private Vector3 targetPosition; // 目標位置
    private bool reachedTarget = false; // 目標位置に到達したかどうか
    private Vector3 moveDirection; // 目標位置への移動方向
    private bool isReflected = false; // 反射中かどうかのフラグ

    void Start()
    {
        // プレイヤーの位置を目標位置として設定
        GameObject player = GameObject.FindWithTag("Player"); // プレイヤーオブジェクトをタグで検索

        if (player != null)
        {
            targetPosition = player.transform.position;
            moveDirection = (targetPosition - transform.position).normalized; // 移動方向を計算
        }
        else
        {
            Debug.LogError("Playerタグを持つオブジェクトが見つかりません！");
        }

        // ShieldControllerがインスペクターで指定されていない場合、自動検索
        if (shieldController == null)
        {
            shieldController = FindObjectOfType<ShieldController>();
            if (shieldController == null)
            {
                Debug.LogError("ShieldController が見つかりません！インスペクターで設定してください。");
            }
        }
    }

    void Update()
    {
        if (!reachedTarget && !isReflected)
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
            // 目標到達後、または反射後に真っ直ぐ進む
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
        // Shieldタグと衝突した場合
        if (other.CompareTag("Shield"))
        {
            if (shieldController != null && shieldController.IsReflecting())
            {
                Reflect(); // 反射処理を呼び出し
                return; // 反射時は破壊せずリターン
            }
        }

        // Playerタグと衝突した場合、または反射モードでないShieldタグと衝突した場合
        if (other.CompareTag("Player") || other.CompareTag("Shield"))
        {
            Debug.Log($"{gameObject.name} が {other.gameObject.tag} と衝突し破壊されました。");
            Destroy(gameObject);
        }
    }

    private void Reflect()
    {
        if (!isReflected)
        {
            isReflected = true;

            // X軸とZ軸の方向を反転
            moveDirection = new Vector3(-moveDirection.x, moveDirection.y, -moveDirection.z);
            Debug.Log($"{gameObject.name} が反射しました。新しい方向: {moveDirection}");
        }
    }
}
