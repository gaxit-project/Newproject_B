using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossScript : MonoBehaviour
{
    [SerializeField] private GameObject fishPrefab;
    [SerializeField] private GameObject rockPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float attackInterval = 2f;
    [SerializeField] private int attackNumber = 2; //攻撃の種類
    [SerializeField] private Slider bossHpSlider;
    int attackType = 1; //攻撃の種類

    private Transform player; //playerの位置を入れる変数
    void Awake()
    {
        // タグが"Player"のオブジェクトを探してTransformを取得
        player = GameObject.FindGameObjectWithTag("Player").transform;
        InvokeRepeating("Attack", attackInterval, attackInterval);
        bossHpSlider.value = 100;

    }

    // Update is called once per frame
    void Update()
    {

        LookAtPlayer();

        if (Input.GetKeyDown(KeyCode.Z))
        {
            //fishShot(); //Zキー入力で弾を発射
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

    void Attack()
    {
        GameObject currentAttack;

        switch (attackType)
        {
            case 1:
                currentAttack = fishPrefab;
                break;
            case 2:
                currentAttack = rockPrefab;
                break;
    
            default:
                currentAttack = null;
                break;
        }

        if (currentAttack != null)
        {
            Instantiate(currentAttack, spawnPoint.position, spawnPoint.rotation);
        }

        // 攻撃タイプを切り替え
        attackType++;
        if (attackType > attackNumber)
        {
            attackType = 1; // 循環するようにリセット
        }
    }


    public void OnTriggerEnter(Collider collision){
        if(collision.gameObject.CompareTag("Rubble")){
            bossHpSlider.value -= 10;
            Debug.Log(bossHpSlider.value);

        }else{
            Debug.Log("何かに当たったよ");
        }
    }
}
