using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseFish : MonoBehaviour
{
    public float speed = 5f; // 移動速度
    public float chasingTime = 7f; //プレイヤーを追跡する秒数

    private Vector3 targetPosition; // 目標位置
    private Vector3 moveDirection; // 目標位置への移動方向
    private bool isChasing = true; //追跡をするかどうか
    [SerializeField] private MeshRenderer meshRenderer; //点滅させる用

    void Start()
    {
        Invoke("StartBlinking", chasingTime); // 指定秒後にオブジェクトの当たり判定が無くなる

    }

    void Update()
    {
        if(isChasing)
        {
            Invoke("DefinePlayerPosition", 0); // 目標位置を更新
            transform.LookAt(new Vector3(targetPosition.x, transform.position.y, targetPosition.z));
            transform.Rotate(0, 90, 0);
            // 目標位置に向かって進む
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        }
        else
        {
            //まっすぐ進む
            transform.position += moveDirection * 0.5f * Time.deltaTime;
        }
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

    private void StartBlinking() //オブジェクトの当たり判定を消す
    {
        isChasing = false;
        gameObject.layer = LayerMask.NameToLayer("BlinkingFish");
        Invoke("Blink", 0);
        Invoke("Disappear", 5f);
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
    private void Disappear() //オブジェクトを破壊
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        // ShieldタグまたはPlayerタグまたはRubbleタグと衝突した場合にオブジェクトを破壊
        if (other.CompareTag("Shield") || other.CompareTag("Player") || other.CompareTag("Rubble"))
        {
            Debug.Log($"{gameObject.name} が {other.gameObject.tag} と衝突し破壊されました。");
            Destroy(gameObject);
        }
    }
}
