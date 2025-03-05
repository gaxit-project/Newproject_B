using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafFish : MonoBehaviour
{
    public float rotationPeriod = 3f; // 回転周期
    public float naturalDisappearTime = 10f; // 自然消滅までの時間

    [SerializeField] private MeshRenderer meshRenderer; //点滅させる用
    private Vector3 rotationAxis; //回転軸(=ボスの座標)
    private bool isAttacking = false; //攻撃した(=ShieldまたはPlayerに触れた)かどうか

    void Start()
    {
        Invoke("Dissappear", naturalDisappearTime); //指定時間後消滅させる
    }

    void Update()
    {
        Invoke("DefineBossPosition", 0f);
        //ボスの周りを回転させる
        transform.RotateAround(rotationAxis, Vector3.up, -360 / rotationPeriod * Time.deltaTime);
    }

    private void DefineBossPosition()
    {
        // ボスの位置を取得
        GameObject boss = GameObject.FindWithTag("Boss"); // タグで検索

        if (boss != null)
        {
            rotationAxis = boss.transform.position;
        }
        else
        {
            Debug.LogError("Bossタグを持つオブジェクトが見つかりません！");
        }
    }
        private void OnTriggerEnter(Collider other)
    {
        // RubbleタグまたはWallタグと衝突した場合にオブジェクトを破壊
        if (other.CompareTag("Rubble") || other.CompareTag("Wall"))
        {
        //    Debug.Log($"{gameObject.name} が {other.gameObject.tag} と衝突し破壊されました。");
        //    Destroy(gameObject);
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
