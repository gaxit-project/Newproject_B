using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafFish : MonoBehaviour
{
    public float speed = 0.01f; // 移動速度
    public float rotationTime = 10f; // 方向転換するまでの間隔
    public float dissappearTime = 30f; // 自然消滅までの時間

    private float additionalAngle; // 方向転換の際使用する

    void Start()
    {
        additionalAngle = Random.Range(-180.0f, 180.0f); // 初期の方向をランダム設定
        Invoke("RotateObject", rotationTime);
        Invoke("Dissappear", dissappearTime);
    }

    void Update()
    {
        var velocity = new Vector3(0f, 0f, speed); // 速度の設定

        // ★ 常に +90度 の回転補正を適用
        transform.rotation = Quaternion.Euler(0f, additionalAngle + Mathf.PingPong(Time.time * 15f, 60f) - 30f, 0f)
                           * Quaternion.Euler(0, 90, 0); // +90度回転を適用

        // ★ 向いている方向に移動 (現在の回転を考慮)
        transform.position += transform.rotation * Vector3.forward * speed;
    }

    private void RotateObject()
    {
        additionalAngle = (additionalAngle + Random.Range(50, 311)) % 360; // 方向変更（360°の範囲に収める）
        Invoke("RotateObject", rotationTime); // 指定秒数後繰り返す
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

    private void Dissappear() // オブジェクトを破壊
    {
        Destroy(gameObject);
    }
}
