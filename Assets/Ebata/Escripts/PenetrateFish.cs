using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Penetratefish : MonoBehaviour
{
    public float speed = 5f; // 移動速度
    public float continueStraightDuration = 4f; // 目標位置到達後に真っ直ぐ進む時間
    public string TargetTag = "Player"; //スクリプトを使いまわす用　目標のオブジェクトのタグを入れる

    private Vector3 targetPosition; // 目標位置
    private bool reachedTarget = false; // 目標位置に到達したかどうか
    private Vector3 moveDirection; // 目標位置への移動方向

    void Start()
    {
        // 目標位置を設定
        GameObject target = GameObject.FindWithTag(TargetTag); // 目標のオブジェクトをタグで検索

        if (target != null)
        {        
            targetPosition = new Vector3(target.transform.position.x, 9f, target.transform.position.z);
            moveDirection = (targetPosition - transform.position).normalized; // 移動方向を計算
            transform.LookAt(new Vector3(targetPosition.x, 0f, targetPosition.z));
            transform.Rotate(-130, 0, 0);
        }
        else
        {
            Debug.LogError("TargetTagに書かれたタグを持つオブジェクトが見つかりません！");
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
        //Playerタグと衝突した場合にオブジェクトを破壊
        if (other.CompareTag("Player") || other.CompareTag("Rubble"))
        {
            Debug.Log($"{gameObject.name} が {other.gameObject.tag} と衝突し破壊されました。");
            Destroy(gameObject);
        }
    }
}
