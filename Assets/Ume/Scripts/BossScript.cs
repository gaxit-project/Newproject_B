using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BossScript : MonoBehaviour
{
    [SerializeField] private GameObject fishPrefab;
    [SerializeField] private GameObject rockPrefab;
    [SerializeField] private GameObject chargePrefab; // 突進用
    [SerializeField] private Transform spawnPoint; //ボスのトランスフォーム
    [SerializeField] private float attackInterval = 2f;
    [SerializeField] private Slider bossHpSlider;
    [SerializeField] private float chargeDistance = 10f;
    [SerializeField] private float chargeSpeed = 10f;
    [SerializeField] private float rotationSpeed = 2f;

    private Transform player;
    private int currentAttackIndex = 0;
    private List<int> attackPattern;
    private bool IsSpecialATC = false;
    private bool isCharging = false;
    private Vector3 originalPosition;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        bossHpSlider.value = 100;
        bossHpSlider.maxValue = 100;
        originalPosition = transform.position;

        UpdateAttackPattern();
        InvokeRepeating("Attack", attackInterval, attackInterval);
    }

    void Update()
    {
        if (!isCharging)
        {
            LookAtPlayer();
        }
        if (bossHpSlider.value <= 0)
        {
            SceneManager.LoadScene("ClearScene"); //HPが0になったらシーン遷移
        }
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
        if (isCharging || attackPattern == null || attackPattern.Count == 0)
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
        isCharging = true;

        // 1. プレイヤーを見るのをやめる
        Quaternion originalRotation = transform.rotation;

        // 2. 左右に30度ずつ揺らす
        float angle = 30f;
        float duration = 0.5f;
        Quaternion leftRotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y - angle, transform.eulerAngles.z);
        Quaternion rightRotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + angle, transform.eulerAngles.z);

        yield return RotateTo(leftRotation, duration);
        yield return RotateTo(rightRotation, duration);
        yield return RotateTo(originalRotation, duration);

        // 3. 突進する（回転なしで現在の向きのまま）
        Vector3 chargeDirection = transform.forward;
        Vector3 targetPosition = transform.position + chargeDirection * chargeDistance;
        yield return MoveTo(targetPosition, chargeSpeed);

        // 4. 元の位置に戻る
        yield return MoveTo(originalPosition, chargeSpeed);

        isCharging = false;
    }

    IEnumerator RotateTo(Quaternion targetRotation, float duration)
    {
        float time = 0f;
        Quaternion startRotation = transform.rotation;
        while (time < duration)
        {
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.rotation = targetRotation;
    }

    IEnumerator MoveTo(Vector3 targetPosition, float speed)
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator SpecialAction()
    {
        if (IsSpecialATC)
        {
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
