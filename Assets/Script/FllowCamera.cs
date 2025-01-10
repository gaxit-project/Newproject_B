using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FllowCamera : MonoBehaviour
{
    public Transform player; // プレイヤーオブジェクトのTransform
    public Vector3 offset; // カメラとプレイヤーのオフセット

    void Start()
    {
        // プレイヤーが設定されていない場合、自動でPlayerタグから探す
        if (player == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
            else
            {
                Debug.LogError("Playerタグを持つオブジェクトが見つかりません！");
            }
        }

        // オフセットを初期化（現在のカメラ位置とプレイヤーの相対位置）
        if (player != null)
        {
            offset = transform.position - player.position;
        }
    }

    void LateUpdate()
    {
        // プレイヤーの位置に基づいてカメラの位置を更新
        if (player != null)
        {
            transform.position = player.position + offset;
        }
    }
}