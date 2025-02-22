using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoDFish : MonoBehaviour // CoDはChange of Direction(方向転換)の略
{
    public float firstSpeed = 8f; // 最初の移動速度
    public float secondSpeed = 24f; // 方向転換後の移動速度
    public float continueStraightDuration = 2f; // プレイヤー位置到達後に真っ直ぐ進む時間
    public float startChangingTime = 2f; // 方向転換するまでの時間

    public bool changePosition = false; // falseならTargetAに、trueならTargetBに向かう

    private Vector3 targetPosition; // 目標位置
    private bool reachedTarget = false; // 目標位置に到達したかどうか
    private Vector3 moveDirection; // 目標位置への移動方向
    private bool startMovingTowardPlayer = false; // 動き始めるかどうか
    private GameObject tar; // 最初の目標を決める際に使用する
    private GameObject player; // 方向転換後の目標を決める際に使用する

    void Start()
    {
        if (!changePosition)
        {
            tar = GameObject.FindWithTag("TargetA");
        }
        else
        {
            tar = GameObject.FindWithTag("TargetB");
        }

        if (tar != null)
        {
            targetPosition = new Vector3 (tar.transform.position.x, 0f, tar.transform.position.z);
            moveDirection = (targetPosition - transform.position).normalized; // 移動方向を計算

            // ★ ターゲットの方を向く（90度回転補正）
            LookAtDirection(targetPosition);
        }
        else
        {
            Debug.LogError("TargetAタグまたはTargetBタグを持つオブジェクトが見つかりません！");
        }

        Invoke("MoveTowardPlayer", startChangingTime);
    }


    void Update()
    {
        if (!startMovingTowardPlayer && !reachedTarget)
        {
            // 目標位置に向かって進む
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, firstSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                reachedTarget = true;
            }
        }
        else if(!startMovingTowardPlayer && reachedTarget)
        {
            // 到達後、真っ直ぐ進む
            transform.position += moveDirection * firstSpeed * Time.deltaTime;
        }
        else if (startMovingTowardPlayer && !reachedTarget)
        {
            // 目標位置に向かって進む（方向転換後）
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, secondSpeed * Time.deltaTime);

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
            transform.position += moveDirection * secondSpeed * Time.deltaTime;
        }
    }

    private void MoveTowardPlayer()
    {
        reachedTarget = false;
        startMovingTowardPlayer = true;

        player = GameObject.FindWithTag("Player"); // プレイヤーオブジェクトをタグで検索
        if (player != null)
        {
            targetPosition = player.transform.position;
            moveDirection = (targetPosition - transform.position).normalized; // 移動方向を計算

            // ★ 方向転換してプレイヤーの方向を向く（修正ポイント）
            LookAtDirection(targetPosition);
        }
        else
        {
            Debug.LogError("Playerタグを持つオブジェクトが見つかりません！");
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
        if (other.CompareTag("Shield") || other.CompareTag("Player") || other.CompareTag("Rubble"))
        {
            Debug.Log($"{gameObject.name} が {other.gameObject.tag} と衝突し破壊されました。");
            Destroy(gameObject);
        }
    }

    // ★ 新しく追加したメソッド: 移動方向を向く処理
    private void LookAtDirection(Vector3 target)
    {
        Vector3 lookDirection = (target - transform.position).normalized;
        lookDirection.y = 0; // 水平方向のみを向く

        // ★ 90度回転を加える
        transform.rotation = Quaternion.LookRotation(lookDirection) * Quaternion.Euler(0, 90, 0);
    }

}
