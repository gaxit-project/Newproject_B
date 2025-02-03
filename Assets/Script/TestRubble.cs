using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRubble : MonoBehaviour
{
    public ShieldController shieldController; // ShieldManagerにアタッチされたShieldControllerを参照
    public float speed = 5f; // 移動速度
    public float continueStraightDuration = 2f; // プレイヤー位置到達後に真っ直ぐ進む時間
    private float initialY; // 初期のY座標を保持
    private Vector3 targetPosition; // 目標位置
    private bool reachedTarget = false; // 目標位置に到達したかどうか
    private Vector3 moveDirection; // 目標位置への移動方向
    private bool isReflected = false; // 反射中かどうかのフラグ

    void Start()
    {
        initialY = transform.position.y; // Y座標を固定

        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            targetPosition = player.transform.position;
            targetPosition.y = initialY;
            moveDirection = (targetPosition - transform.position).normalized * speed; // 速度を掛ける
        }
        else
        {
            Debug.LogError("Playerタグを持つオブジェクトが見つかりません！");
        }

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
            Vector3 newPosition = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            newPosition.y = initialY;
            transform.position = newPosition;

            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                reachedTarget = true;
                StartCoroutine(ContinueStraight());
            }
        }
        else
        {
            Vector3 newPosition = transform.position + moveDirection * Time.deltaTime;
            newPosition.y = initialY;
            transform.position = newPosition;
        }
    }

    private IEnumerator ContinueStraight()
    {
        yield return new WaitForSeconds(continueStraightDuration);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
{
    if (other.CompareTag("Shield"))
    {
        if (shieldController != null && shieldController.IsReflecting())
        {
            // 近い衝突点を取得し、そこから法線を計算
            Vector3 collisionPoint = other.ClosestPoint(transform.position);
            Vector3 collisionNormal = (transform.position - collisionPoint).normalized;

            Reflect(collisionNormal);
            return;
        }
    }

    if (other.CompareTag("Player") || other.CompareTag("Shield"))
    {
        Debug.Log($"{gameObject.name} が {other.gameObject.tag} と衝突し破壊されました。");
        Destroy(gameObject);
    }
}


    private void Reflect(Vector3 collisionNormal)
    {
        if (!isReflected)
        {
            isReflected = true;

            // 反射ベクトルを計算し、速度を維持する
            moveDirection = Vector3.Reflect(moveDirection.normalized, collisionNormal).normalized * speed;

            moveDirection.y = 0; // Y方向の動きを防ぐ

            Debug.Log($"{gameObject.name} が反射しました。新しい方向: {moveDirection}");
        }
    }
}
