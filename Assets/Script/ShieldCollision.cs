using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldCollision : MonoBehaviour
{
    public ShieldController shieldController; // ShieldControllerへの参照

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"トリガーに入ったオブジェクト: {other.gameObject.name}");

        // Fish または Rubble タグのオブジェクトと接触した場合
        if (other.gameObject.CompareTag("Fish") || other.gameObject.CompareTag("Rubble"))
        {
            shieldController?.ReduceShieldHP(); // HPを減少させる
            Debug.Log($"Shieldが {other.gameObject.tag} とトリガーで接触しました");
        }
        else
        {
            Debug.Log($"対応しないオブジェクトがトリガーに入りました: {other.gameObject.tag}");
        }
    }
}
