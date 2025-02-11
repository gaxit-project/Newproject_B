using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class LeafFish : MonoBehaviour
{
    public float speed = 0.01f; //移動速度
    public float rotationTime = 10f; //方向転換するまでの間隔
    public float dissappearTime = 30f; //自然消滅までの時間

    private float additionalAngle; //方向転換の際使用する
    
    // Start is called before the first frame update
    void Start()
    {
        additionalAngle = Random.Range(-180.0f, 179.9f); //ランダムで方向を決定
        Invoke("RotateObject", rotationTime);
        Invoke("Dissappear", dissappearTime);
    }

    // Update is called once per frame
    void Update()
    {
        var verocity = new Vector3(0f, 0f, speed); //速度の設定
        //葉っぱのような動きをしながら移動する
        transform.rotation = Quaternion.Euler(0f, Mathf.PingPong(Time.time * 15f, 60f) - 30f + additionalAngle, 0f);
        transform.position += transform.rotation * verocity;
    }

    private void RotateObject()
    {
        additionalAngle += Random.Range(50, 311); //ランダムで方向を決定
        //値が大きくなりすぎないようにする
        if(additionalAngle >= 360)
        {
            additionalAngle -= 360;
        }    
        Invoke("RotateObject", rotationTime); //指定秒数後繰り返す
    }

    private void OnTriggerEnter(Collider other)
    {
        // ShieldタグまたはPlayerタグと衝突した場合にオブジェクトを破壊
        if (other.CompareTag("Shield") || other.CompareTag("Player"))
        {
            Debug.Log($"{gameObject.name} が {other.gameObject.tag} と衝突し破壊されました。");
            Destroy(gameObject);
        }
    }
    private void Dissappear() //オブジェクトを破壊
    {
        Destroy(gameObject);
    }
}
