using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldCollision : MonoBehaviour
{
    public ShieldController shieldController; // ShieldControllerへの参照

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"トリガーに入ったオブジェクト: {other.gameObject.name}");

        if (other.gameObject.CompareTag("Fish"))
        {
            shieldController?.ReduceShieldHP(); // HPを減少させる
            Debug.Log("ShieldがFishとトリガーで接触しました");
        }
        else
        {
            Debug.Log($"Fish以外のオブジェクトがトリガーに入りました: {other.gameObject.tag}");
        }
    }
}
