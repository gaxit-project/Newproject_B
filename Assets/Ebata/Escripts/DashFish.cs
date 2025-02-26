using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class DashFish : MonoBehaviour
{
    public float speed = 30f; // 移動速度
    public float continueStraightDuration = 2f; // プレイヤー位置到達後に真っ直ぐ進む時間
    public float startDashingTime = 3f; // 進み始めるまでの時間

    [SerializeField] private MeshRenderer meshRenderer; //点滅させる用
    private bool isAttacking = false; //攻撃した(=ShieldまたはPlayerに触れた)かどうか
    private Vector3 targetPosition; // 目標位置
    private bool reachedTarget = false; // 目標位置に到達したかどうか
    private Vector3 moveDirection; // 目標位置への移動方向
    private bool startMoving = false; // 動き始めるかどうか
    private bool stopDefining = false; // 目標位置の更新を止めるかどうか

    void Start()
    {
        Invoke("MoveTowardPlayer", startDashingTime);  // 指定時間後に動き始める
    }

    void Update()
    {
        GameObject player = GameObject.FindWithTag("Player"); // プレイヤーを取得

        if (!startMoving)
        {
            if (player != null && !stopDefining)
            {
                targetPosition = player.transform.position;
                moveDirection = (targetPosition - transform.position).normalized; // 移動方向を計算

                // プレイヤーの方を向く
                Vector3 lookDirection = (player.transform.position - transform.position).normalized;
                lookDirection.y = 0; // 水平方向のみに向きを調整
                Quaternion lookRotation = Quaternion.LookRotation(lookDirection);

                // **モデルが横向き（X軸前方）なら回転補正**
                transform.rotation = lookRotation * Quaternion.Euler(0, 90, 0);  
            }
            else if (!stopDefining)
            {
                Debug.LogError("Playerタグを持つオブジェクトが見つかりません！");
            }
        }
        else if (!reachedTarget && !isAttacking)
        {
            // 目標位置に向かって進む
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        }

        if (!reachedTarget && !isAttacking)
        {
            // 目標に到達したらフラグを設定
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

    private async void MoveTowardPlayer()
    {
        stopDefining = true;
        await Task.Delay(300); // 300ミリ秒(0.3秒)遅らせる
        startMoving = true;
    }

    private IEnumerator ContinueStraight()
    {
        // 指定時間後にオブジェクトを破壊
        yield return new WaitForSeconds(continueStraightDuration);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        // RubbleタグまたはWallタグと衝突した場合にオブジェクトを破壊
        if (other.CompareTag("Rubble") || other.CompareTag("Wall"))
        {
            Debug.Log($"{gameObject.name} が {other.gameObject.tag} と衝突し破壊されました。");
            Destroy(gameObject);
        }
        // ShieldタグまたはPlayerタグと衝突した場合にオブジェクトを点滅
        else if((other.CompareTag("Player") || other.CompareTag("Shield")) && !isAttacking)
        {
            Debug.Log($"{gameObject.name} が {other.gameObject.tag} と衝突しました。");
            isAttacking = true;
            gameObject.layer = LayerMask.NameToLayer("BlinkingFish");
            Invoke("Blink", 0);
        }
    }
    private void Blink() //点滅させる
    {
        if(meshRenderer.enabled)
        {
            meshRenderer.enabled = false;
        }
        else
        {
            meshRenderer.enabled = true;
        }
        Invoke("Blink", 0.1f);
    }
}