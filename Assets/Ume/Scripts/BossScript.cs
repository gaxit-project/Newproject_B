using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossScript : MonoBehaviour
{
    [SerializeField] private GameObject fishPrefab;
    [SerializeField] private GameObject rockPrefab;
    [SerializeField] private GameObject chargePrefab; // 突進用
    [SerializeField] private Transform spawnPoint; //ボスのトランスフォーム
    [SerializeField] private float attackInterval = 2f;
    [SerializeField] private Slider bossHpSlider;

    private Transform player;
    private int currentAttackIndex = 0;
    private List<int> attackPattern;
    private bool IsSpecialATC = false;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        bossHpSlider.value = 100;
        bossHpSlider.maxValue = 100;

        UpdateAttackPattern();
        InvokeRepeating("Attack", attackInterval, attackInterval);
    }

    void Update()
    {
        LookAtPlayer();
    }

    private void LookAtPlayer()
    {
        if (player != null)
        {
            transform.LookAt(player);
        }
    }

    void Attack()
    {
        if (attackPattern == null || attackPattern.Count == 0) 
        {
            return;
        }

        GameObject currentAttack = null;

        switch (attackPattern[currentAttackIndex])
        {
            case 1: // さかな
                currentAttack = fishPrefab;
                break;
            case 2: // 岩
                currentAttack = rockPrefab;
                break;
            case 3: // 突進
                StartCoroutine(ChargeAttack());
                break;
            case 4: // 特殊行動（盾破壊）
                StartCoroutine(SpecialAction());
                break; // 特殊行動は通常攻撃と別処理なのでここで終了
        }

        if (currentAttack != null)
        {
            Vector3 spawnPosition = spawnPoint.position + spawnPoint.forward * 10f;
            Instantiate(currentAttack, spawnPosition, spawnPoint.rotation);
        }

        // 次の攻撃へ
        currentAttackIndex = (currentAttackIndex + 1) % attackPattern.Count;
    }

    IEnumerator ChargeAttack()
    {
        // 突進の処理（ボスがプレイヤーに突進）
        Debug.Log("ボスが突進！");
        yield return new WaitForSeconds(1f); // 突進アニメーション用
    }

    IEnumerator SpecialAction()
    {
        if(IsSpecialATC) {
            yield break;
        }
        // 盾を壊す処理
        Debug.Log("ボスが咆哮！ 盾破壊！");
        yield return new WaitForSeconds(2f);
        Debug.Log(attackPattern.Count);

        // 盾復活後の攻撃パターン更新
         UpdateAttackPattern();
        IsSpecialATC = true;

    }

    public void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Rubble"))
        {
            bossHpSlider.value -= 10;
            Debug.Log(bossHpSlider.value);
            Destroy(collision.gameObject);
            UpdateAttackPattern(); // HPが減ったら攻撃パターンを更新
        }
    }

    void UpdateAttackPattern()
    {
        attackPattern = new List<int>();

        if (bossHpSlider.value > 50) // HP 10~6
        {
            attackPattern.AddRange(new List<int> { 2, 1, 2, 1, 1, 3, 2 }); // 岩⇒さかな⇒岩⇒さかな⇒さかな⇒突進⇒岩
        }
        else // HP 5~
        {
            attackPattern.Add(4); // 特殊行動
            attackPattern.AddRange(new List<int> { 2, 1, 3, 3, 1, 2, 3 }); // 盾復活後のパターン
        }

        currentAttackIndex = 0; // パターンを最初から開始
    }
}
