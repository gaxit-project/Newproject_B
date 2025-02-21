using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BossScript : MonoBehaviour
{
    [SerializeField] private GameObject normalFishPrefab;
    [SerializeField] private GameObject chaseFishPrefab;
    [SerializeField] private GameObject dashFishPrefab;
    [SerializeField] private GameObject towWayFishPrefabA;
    [SerializeField] private GameObject towWayFishPrefabB;
    [SerializeField] private GameObject leafFishPrefab;
    [SerializeField] private GameObject panetratingFishPrefab;






    [SerializeField] private GameObject rockPrefab;

    [SerializeField] private Transform spawnPoint; //ボスのトランスフォーム
    [SerializeField] private float attackInterval = 2f;
    [SerializeField] private Slider bossHpSlider;
    [SerializeField] private float chargeDistance = 10f;
    [SerializeField] private float chargeSpeed = 10f;
    //[SerializeField] private float rotationSpeed = 2f;

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
            case 1: // 時期狙いさかな
                currentAttack = normalFishPrefab;
                break;
            case 2: // 岩
                currentAttack = rockPrefab;
                break;
            case 3: //追跡さかな
                currentAttack = chaseFishPrefab;
                break;
            case 4: //突進さかな
                currentAttack = dashFishPrefab;
                break;
            case 5: //2方向時期さかな
                currentAttack = towWayFishPrefabA;
                break;
            case 6: //木の葉さかな
                currentAttack = leafFishPrefab;
                break;
            case 7: //盾貫通さかな
                currentAttack = panetratingFishPrefab;
                break;
            case 50: // 突進
                StartCoroutine(ChargeAttack());
                break;
            case 99: // 特殊行動（盾破壊）
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

    private Coroutine chargeMoveCoroutine; // 突進のMoveTo用のコルーチンを管理

    IEnumerator ChargeAttack()
    {
        isCharging = true;
        Quaternion originalRotation = transform.rotation;
        chargeDistance = Vector3.Distance(transform.position, player.position);

        float angle = 30f;
        float duration = 0.5f;
        Quaternion leftRotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y - angle, transform.eulerAngles.z);
        Quaternion rightRotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + angle, transform.eulerAngles.z);

        yield return RotateTo(leftRotation, duration);
        yield return RotateTo(rightRotation, duration);
        yield return RotateTo(originalRotation, duration);

        // 突進を開始し、コルーチンを管理
        Vector3 chargeDirection = transform.forward;
        Vector3 targetPosition = transform.position + chargeDirection * chargeDistance;
        chargeMoveCoroutine = StartCoroutine(MoveTo(targetPosition, chargeSpeed));

        yield return chargeMoveCoroutine; // 突進が終わるのを待つ

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
        float time = 0f;
        float maxDuration = 3f; // 3秒経過したら強制終了

        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            time += Time.deltaTime;

            if (time >= maxDuration)
            {
                break;
            }
            yield return null;
        }

        transform.position = targetPosition; // 最後に確実に目標地点へ
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
        if (collision.gameObject.CompareTag("Rubble") && !isCharging)
        {
            bossHpSlider.value -= 10;
            Debug.Log(bossHpSlider.value);
            Destroy(collision.gameObject);

            bool lastAttack = false;
            if (bossHpSlider.value <= 50 && lastAttack == false)
            {
                UpdateAttackPattern(); // HPが減ったら攻撃パターンを更新
                lastAttack = true;
            }
        }
        else if (collision.gameObject.CompareTag("Rubble") && isCharging)
        {
            // 突進のMoveTo()だけを止める
            if (chargeMoveCoroutine != null)
            {
                StopCoroutine(chargeMoveCoroutine);
                chargeMoveCoroutine = null;
            }

            isCharging = false; // 突進を終了

            // 元の位置に戻る
            StartCoroutine(MoveTo(originalPosition, chargeSpeed));
        }
    }


    void UpdateAttackPattern()
    {
        attackPattern = new List<int>();

        if (bossHpSlider.value > 50) // HP 10~6
        {
            attackPattern.AddRange(new List<int> { 2, 1, 2, 50, 3, 4, 2, 3, 4, 50 }); // 岩⇒ノーマルさかな(青)⇒岩⇒突進⇒追尾さかな(赤)⇒突進さかな(黄)⇒岩⇒赤さかな+青さかな⇒突進
        }
        else // HP 5~
        {
            attackPattern.Add(99); // 特殊行動
            attackPattern.AddRange(new List<int> { 5, 2, 6, 6, 6, 50, 7, 1, 5, 2, 3, 4, 5, 50, 50, 7, 3, 4, 5 }); // 2方向突進さかな(緑)⇒岩⇒木の葉さかな(橙)*3⇒突進⇒盾貫通さかな(紫)⇒青さかな+緑さかな
                                                                                                                  //  ⇒岩⇒赤さかな+黄さかな⇒2方向に紫さかな⇒突進⇒突進⇒2方向に紫さかな+赤さかな⇒黄さかな+緑さかな
        }

        currentAttackIndex = 0; // パターンを最初から開始
    }
}
