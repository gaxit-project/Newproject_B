using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockScript : MonoBehaviour
{
    [SerializeField] private float speed = 10f; // 移動速度
    [SerializeField] private float lifeTime = 5f; // 存在時間
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lifeTime); // 一定時間後に消滅
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // プレイヤーにダメージを与える処理をここに書く
            Destroy(gameObject); // 衝突したら消滅
        }
    }
}
