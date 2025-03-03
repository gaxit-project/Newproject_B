using UnityEngine;
using System.Collections;

public class MiniRubble : MonoBehaviour
{
    public ShieldController shieldController; // ShieldManagerにアタッチされたShieldControllerを参照
    private Vector3 moveDirection;
    private bool isReflected = false;
    public float stopDelay = 0.5f;
    private float initialY;
    public float speed = 2.5f; // 速度を `TestRubble` の半分に調整
    private bool canTrigger = false; // 衝突判定を一時的に無効化

    void Start()
    {
        initialY = transform.position.y;
        StartCoroutine(StopMovementAfterDelay(stopDelay));
        StartCoroutine(EnableTriggerAfterDelay(0.5f)); // 0.5秒後にOnTriggerEnterを有効化
    }

    void Update()
    {
        if (moveDirection != Vector3.zero)
        {
            Vector3 newPosition = transform.position + moveDirection * Time.deltaTime;
            newPosition.y = initialY; // Y軸の固定
            transform.position = newPosition;
        }
    }

    public void Initialize(Vector3 direction, bool reflected)
    {
        moveDirection = direction;
        isReflected = reflected;
    }

    private IEnumerator StopMovementAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        moveDirection = Vector3.zero;
    }

    private IEnumerator EnableTriggerAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        canTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!canTrigger) return; // 0.3秒以内は衝突処理を無視する

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

        if (other.CompareTag("Player") || other.CompareTag("Shield") || other.CompareTag("Rubble")|| other.CompareTag("Wall")|| other.CompareTag("Boss"))
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

            // ここに反射音SE
            SoundSE.Reflect();

            // 1秒から1.5秒の間でランダムな遅延時間を生成
            float randomDelay = Random.Range(1f, 1.5f);

            // ランダムな遅延時間後に移動を停止するコルーチンを開始
            StartCoroutine(StopMovementAfterDelay(randomDelay));
        }
    }
}
