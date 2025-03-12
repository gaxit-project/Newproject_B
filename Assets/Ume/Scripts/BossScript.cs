using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BossScript : MonoBehaviour
{
    Rigidbody rb;

    private SkinnedMeshRenderer skinnedMeshRenderer;
    [SerializeField] private float blinkDuration = 0.2f; // 点滅の長さ
    [SerializeField] private int blinkCount = 3;        // 点滅の回数

    [SerializeField] private FishContoller fishContoller; // FishManagerにアタッチされたFishControllerを参照
    [SerializeField] private ShieldController shieldController; // ShieldManagerにアタッチされたShieldControllerを参照

    [SerializeField] private Transform spawnPoint; //ボスのトランスフォーム
    [SerializeField] private float attackInterval = 2f;
    [SerializeField] private Slider bossHpSlider;
    [SerializeField] private float chargeSpeed = 10f;
    //[SerializeField] private float rotationSpeed = 2f;

    private Transform player;
    private int currentAttackIndex = 0;
    private List<int> attackPattern;
    private bool IsSpecialATC = false;
    private bool isCharging = false;
    private bool isCountered = false;
    private Vector3 originalPosition;
    private bool lastAttack = false; //攻撃パターンを変更する用
    private Vector3 chargedPosition; //突進攻撃する前のポジションを入れる
    private float chargeDistance;
    private Queue<Vector3> playerPositions = new Queue<Vector3>();
    [SerializeField] private float lookDelay = 1f; // 1秒遅らせる



    //アニメーション関連の変数
    private Animator bossAnim;
    string canHit;
    string chargeAttack;
    string deathTrigger;
    string damaged;
    string attack;
    string counter;
    string walk;

    float lastAtkSecond;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        player = GameObject.FindGameObjectWithTag("Player").transform;
        bossHpSlider.value = 100;
        bossHpSlider.maxValue = 100;
        originalPosition = transform.position;

        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();

        UpdateAttackPattern();
        InvokeRepeating("Attack", attackInterval, attackInterval);

        bossAnim = GetComponent<Animator>();
        canHit = "CanHit";
        chargeAttack = "ChargingAttack";
        deathTrigger = "DeathTrigger";
        damaged = "Damaged";
        attack = "Attack";
        counter = "Counter";
        walk = "Walk";
    }

    public bool GetisCharging()
    {
        return isCharging;
    }

    public float GetBossHP()
    {
        return bossHpSlider.value;
    }

    void Update()
    {
        if (bossHpSlider.value <= 50 && !lastAttack)
        {
            UpdateAttackPattern();
            lastAttack = true;
        }
        if (!isCharging)
        {
            UpdatePlayerPosition();
            LookAtDelayedPlayer();
        }
        if (bossHpSlider.value <= 0 && lastAtkSecond == 0)
        {
            lastAtkSecond += Time.deltaTime;
            StartCoroutine(LastAttack());
            SoundBGM.StopBGM();
            if (lastAtkSecond > 10)
            {
                //   SceneManager.LoadScene("ClearScene"); //HPが0になったらシーン遷移
            }
        }
    }

    private void UpdatePlayerPosition()
    {
        if (player == null) return;

        playerPositions.Enqueue(player.position);

        // 1秒分のデータを保持（フレームレート60FPSなら約60個）
        if (playerPositions.Count > Mathf.CeilToInt(lookDelay / Time.deltaTime))
        {
            playerPositions.Dequeue();
        }
    }

    private void LookAtDelayedPlayer()
    {
        if (playerPositions.Count > 0)
        {
            Vector3 delayedPosition = playerPositions.Peek(); // 1秒前の位置を取得

            // 現在の向き
            Quaternion currentRotation = transform.rotation;
            // 目標の向き
            Quaternion targetRotation = Quaternion.LookRotation(delayedPosition - transform.position);
            // ゆっくり回転（数値を小さくするとさらに遅くなる）
            float rotationSpeed = 1.5f; // 数値を小さくすると回転が遅くなる
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationSpeed * Time.deltaTime);
            bossAnim.SetBool(walk, true);
        }
        else
        {
            bossAnim.SetBool(walk, false);
        }
    }

    void Attack()
    {
        isCountered = false;
        if (isCharging || attackPattern == null || attackPattern.Count == 0)
        {
            return;
        }

        //GameObject currentAttack = null;

        switch (attackPattern[currentAttackIndex])
        {
            case 1: // 時期狙いさかな
                fishContoller.spawnNormalFish();
                break;
            case 2: // 岩
                fishContoller.spawnRubble();
                break;
            case 3: //追跡さかな
                fishContoller.spawnChaseFish();
                break;
            case 4: //突進さかな
                fishContoller.spawnDashFish();
                break;
            case 5: //2方向時期さかな
                fishContoller.spawnCoDFish();
                break;
            case 6: //木の葉さかな
                fishContoller.spawnLeafFish();
                break;
            case 7: //盾貫通さかな
                fishContoller.spawnPenetrateFish();
                break;
            case 8: //2方向盾貫通さかな
                fishContoller.spawnTwoWayPenetrateFish();
                break;
            case 9: //追跡さかな＋突進さかな
                fishContoller.spawnChaseFish();
                fishContoller.spawnDashFish();
                break;
            case 10: //時期狙いさかな＋2方向さかな
                fishContoller.spawnNormalFish();
                fishContoller.spawnCoDFish();
                break;
            case 11: //追尾さかな+突進さかな(11)
                fishContoller.spawnChaseFish();
                fishContoller.spawnDashFish();
                break;
            case 12: //2方向盾貫通さかな+追尾さかな(12)
                fishContoller.spawnTwoWayPenetrateFish();
                fishContoller.spawnChaseFish();
                break;
            case 13: //突進さかな + 2方向さかな(13)
                fishContoller.spawnDashFish();
                fishContoller.spawnCoDFish();
                break;
            case 50: // 突進
                StartCoroutine(ChargeAttack());
                break;
            case 99: // 特殊行動（盾破壊）
                StartCoroutine(SpecialAction());
                break; // 特殊行動は通常攻撃と別処理なのでここで終了
        }

        /*if (currentAttack != null)
        {
            Vector3 spawnPosition = spawnPoint.position + spawnPoint.forward * 10f;
            Instantiate(currentAttack, spawnPosition, spawnPoint.rotation);
        }*/

        // 次の攻撃へ
        currentAttackIndex = (currentAttackIndex + 1) % attackPattern.Count;
    }

    private Coroutine chargeMoveCoroutine; // 突進のMoveTo用のコルーチンを管理

    IEnumerator ChargeAttack()
    {
        isCharging = true;
        CancelInvoke("Attack");
        chargedPosition = transform.position;
        Quaternion originalRotation = transform.rotation;
        chargeDistance = Vector3.Distance(transform.position, player.position);
        bossAnim.SetTrigger(chargeAttack);
        SoundSE.BossDashAttack();

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
        SoundSE.BossChargingAttack();

        //噛みつくアニメーション
        bossAnim.SetTrigger(canHit);

        yield return chargeMoveCoroutine; // 突進が終わるのを待つ


        isCharging = false;
        StartCoroutine(WaitTime(3));
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
        if (collision.gameObject.CompareTag("Rubble") && isCountered)
        {
            // 体力変更，カウンターアニメーション追加
            bossHpSlider.value -= 10;
            bossAnim.SetTrigger(damaged);
            SoundSE.BossCounteredDamage();
        }
        else if (collision.gameObject.CompareTag("Rubble") && !isCharging)
        {
            TestRubble rubble = collision.gameObject.GetComponent<TestRubble>();
            if (rubble != null && rubble.isReflected)
            {
                bossHpSlider.value -= 5;
                bossAnim.SetTrigger(damaged);
                SoundSE.BossRockDamage();

                //Debug.Log($"Boss HP: {bossHpSlider.value}");
                //Destroy(collision.gameObject);
            }
        }
        else if (collision.gameObject.CompareTag("Wall") && isCountered)
        {
            bossHpSlider.value -= 10;
            rb.velocity = Vector3.zero;
            //transform.position = new Vector3(0, 0, 0);
            // 元の位置に戻る
            StartCoroutine(MoveTo(new Vector3(0, 0, 0), chargeSpeed * 0.2f));
            isCountered = false;  // ここでフラグを変更

        }
        else if (collision.gameObject.CompareTag("Shield") && shieldController.IsReflecting())
        {
            if (isCountered) return;
            isCountered = true;  // ここでフラグを変更
            rb.velocity = Vector3.zero;  // 速度をリセット
            Vector3 counteredForce = -transform.forward * 30;

            // 攻撃を停止
            CancelInvoke("Attack");

            // 突進のMoveTo()だけを止める
            if (chargeMoveCoroutine != null)
            {
                StopCoroutine(chargeMoveCoroutine);
                chargeMoveCoroutine = null;
            }

            // 体力変更，カウンターアニメーション追加
            bossHpSlider.value -= 2;
            bossAnim.SetTrigger(counter);

            //rb.AddForce(counteredForce);
            StartCoroutine(MoveTo(counteredForce, chargeSpeed * 0.5f));

            StartCoroutine(WaitTime(3));
            isCharging = false; // 突進を終了
        }
    }
    /*void OnCollisionEnter(Collision collision)
    {
        transform.position = new Vector3(0, 0, 0);
        if (collision.gameObject.CompareTag("Wall"))
        {

        }
        if (collision.gameObject.CompareTag("Wall"))
        {
            rb.velocity = Vector3.zero;

            transform.position = new Vector3(0, 0, 0);
            // 元の位置に戻る
            StartCoroutine(MoveTo(new Vector3(0, 0, 0), chargeSpeed * 10f));
        }
    }*/

    IEnumerator WaitTime(int second)
    {
        yield return new WaitForSeconds(second);
        //rb.velocity = Vector3.zero;  // 速度をリセット
        InvokeRepeating("Attack", attackInterval, attackInterval); // 攻撃を再開

    }

    IEnumerator BlinkEffect()
    {
        for (int i = 0; i < blinkCount; i++)
        {
            skinnedMeshRenderer.enabled = false; // SkinnedMeshRenderer を非表示
            yield return new WaitForSeconds(blinkDuration);
            skinnedMeshRenderer.enabled = true; // SkinnedMeshRenderer を表示
            yield return new WaitForSeconds(blinkDuration);
        }
    }


    void UpdateAttackPattern()
    {
        attackPattern = new List<int>();

        if (bossHpSlider.value > 50) // HP 10~6
        {
            attackPattern.AddRange(new List<int> { 2, 1, 2, 50, 3, 4, 2, 9, 50 }); // 岩⇒ノーマルさかな(青)⇒岩⇒突進⇒追尾さかな(赤)⇒突進さかな(黄)⇒岩⇒追尾さかな+ノーマルさかな⇒突進
        }
        else // HP 5~
        {
            attackPattern.Add(99); // 特殊行動
            attackPattern.AddRange(new List<int> { 5, 2, 6, 50, 7, 10, 2, 11, 8, 50, 50, 12, 13 }); // 2方向突進さかな(緑)⇒岩⇒木の葉さかな(橙)*3⇒突進⇒盾貫通さかな(紫)⇒ノーマルさかな+2方向突進さかな
                                                                                                    //  ⇒岩⇒追尾さかな+突進さかな(11)⇒2方向盾貫通さかな⇒突進⇒突進⇒2方向盾貫通さかな+追尾さかな(12)⇒突進さかな+2方向さかな(13)
        }

        currentAttackIndex = 0; // パターンを最初から開始
    }

    IEnumerator LastAttack()
    {
        CancelInvoke("Attack");
        float speed = 10f;
        Time.timeScale = 0.4f;
        bossAnim.SetTrigger(deathTrigger);
        while (Vector3.Distance(transform.position, player.position) > 20f)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            yield return null; // 1フレーム待つ
        }

        //Time.timeScale = 0.25f;
        bossAnim.SetTrigger(canHit);
        yield return new WaitForSecondsRealtime(7);
        Time.timeScale = 1f;
        SceneManager.LoadScene("ClearScene"); //HPが0になったらシーン遷移
    }
}
