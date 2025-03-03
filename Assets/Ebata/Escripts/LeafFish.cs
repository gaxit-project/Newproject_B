using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafFish : MonoBehaviour
{
    public float speed = 3f; // 移動速度
    public float rotationTime = 10f; // 方向転換するまでの間隔
    public float naturalDisappearTime = 30f; // 自然消滅までの時間

    [SerializeField] private MeshRenderer meshRenderer; //点滅させる用
    private bool isAttacking = false; //攻撃した(=ShieldまたはPlayerに触れた)かどうか
    private float additionalAngle; // 方向転換の際使用する

    void Start()
    {
        additionalAngle = Random.Range(-180.0f, 180.0f); // 初期の方向をランダム設定
        Invoke("RotateObject", rotationTime);
        Invoke("Dissappear", naturalDisappearTime);
    }

    void Update()
    {
        var velocity = new Vector3(0f, 0f, speed * Time.deltaTime); // 速度の設定

        // ★ 常に +90度 の回転補正を適用
        transform.rotation = Quaternion.Euler(0f, additionalAngle + Mathf.PingPong(Time.time * 15f, 60f) - 30f, 0f)
                           * Quaternion.Euler(0, 90, 0); // +90度回転を適用

        // ★ 向いている方向に移動 (現在の回転を考慮)
        transform.position += transform.rotation * Vector3.forward * speed * Time.deltaTime;
    }

    private void RotateObject()
    {
        if(!isAttacking)
        {
            additionalAngle = (additionalAngle + Random.Range(50, 311)) % 360; // 方向変更（360°の範囲に収める）
            Invoke("RotateObject", rotationTime); // 指定秒数後繰り返す
        }
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
    
    private void Dissappear() // オブジェクトを破壊
    {
        Destroy(gameObject);
    }
}
