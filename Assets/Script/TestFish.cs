using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowardsPlayer : MonoBehaviour
{
    public float speed = 5f; // 移動速度
    public float continueStraightDuration = 2f; // プレイヤー位置到達後に真っ直ぐ進む時間

    [SerializeField] private MeshRenderer meshRenderer; //点滅させる用
    private bool isAttacking = false; //攻撃した(=ShieldまたはPlayerに触れた)かどうか
    private Vector3 targetPosition; // 目標位置
    private bool reachedTarget = false; // 目標位置に到達したかどうか
    private Vector3 moveDirection; // 目標位置への移動方向
    private GameObject player; //プレイヤーの位置を得る際に使用する
    private int[] array ={-5, 5}; //攻撃後方向転換する際に利用する

    void Start()
    {
        // プレイヤーの位置を目標位置として設定
        player = GameObject.FindWithTag("Player"); // プレイヤーオブジェクトをタグで検索

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
        if (!reachedTarget && !isAttacking)
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
            
            player = GameObject.FindWithTag("Player");
            if(Mathf.Abs(transform.position.x - player.transform.position.x) <= Mathf.Abs(transform.position.z - player.transform.position.z))
            {
                targetPosition = player.transform.position + Vector3.right * array[UnityEngine.Random.Range(0, 2)];
            }
            else
            {
                targetPosition = player.transform.position + Vector3.forward * array[UnityEngine.Random.Range(0, 2)];
            }
            moveDirection = (targetPosition - transform.position).normalized; // 移動方向を計算
            transform.LookAt(new Vector3(targetPosition.x, transform.position.y, targetPosition.z));
            // 初期向きが右（X+方向）になるため、Y軸を90度回転
            transform.Rotate(0, 90, 0);

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
