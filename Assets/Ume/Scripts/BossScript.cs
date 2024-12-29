using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : MonoBehaviour
{
    public GameObject fishPrefab;
    public float fishSpeed = 10;
    private Transform player; //playerの位置を入れる変数
    // Start is called before the first frame update
    void Start()
    {

    }
    void Awake()
    {
        // タグが"Player"のオブジェクトを探してTransformを取得
        player = GameObject.FindGameObjectWithTag("Player").transform;

    }

    // Update is called once per frame
    void Update()
    {

        LookAtPlayer();
        if (Input.GetKeyDown(KeyCode.Z))
        {
            fishShot(); //Zキー入力で弾を発射
        }
    }

    private void LookAtPlayer()
    {
        if (player != null)
        {
            // Playerの方向を向く
            transform.LookAt(player);
        }
    }

    private void fishShot()
    {
        GameObject newFish = Instantiate(fishPrefab, this.transform.position, Quaternion.identity);
        Rigidbody fishRigidbody = newFish.GetComponent<Rigidbody>();
        fishRigidbody.AddForce(this.transform.forward * fishSpeed, ForceMode.Impulse);
        Destroy(newFish, 10); //10秒後に削除
    }
}
