using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BossScript : MonoBehaviour
{
    private SkinnedMeshRenderer skinnedMeshRenderer;
    [SerializeField] private float blinkDuration = 0.2f; // 点滅の長さ
    [SerializeField] private int blinkCount = 3;        // 点滅の回数

    [SerializeField] private FishContoller fishContoller; // FishManagerにアタッチされたFishControllerを参照
    [SerializeField] private ShieldController shieldController; // ShieldManagerにアタッチされたShieldControllerを参照

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
    private bool lastAttack = false; //攻撃パターンを変更する用
    private Vector3 chargedPosition; //突進攻撃する前のポジションを入れる

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
        chargedPosition = transform.position;
        Quaternion originalRotation = transform.rotation;
        chargeDistance = Vector3.Distance(transform.position, player.position);
        bossAnim.SetTrigger(chargeAttack);

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

        //噛みつくアニメーション
        bossAnim.SetTrigger(canHit);

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
            TestRubble rubble = collision.gameObject.GetComponent<TestRubble>();
            if (rubble != null && rubble.isReflected)
            {
                bossHpSlider.value -= 5;
                //StartCoroutine(BlinkEffect());
                bossAnim.SetTrigger(damaged);

                //Debug.Log($"Boss HP: {bossHpSlider.value}");
                //Destroy(collision.gameObject);

                /*if (bossHpSlider.value <= 50 && !lastAttack)
                {
                    UpdateAttackPattern();
                    lastAttack = true;
                }*/
            }
        }
        else if (collision.gameObject.CompareTag("Rubble") && isCharging)
        {

            //Destroy(collision.gameObject); //岩を消す用，エフェクトを追加してそれっぽく見せるように

            isCharging = false; // 突進を終了


        }
        else if (collision.gameObject.CompareTag("Wall"))// && !isCharging)
        {
            Vector3 wallPosition = new Vector3(collision.transform.position.x, 0, collision.transform.position.z);
            // 突進のMoveTo()だけを止める
            if (chargeMoveCoroutine != null)
            {
                StopCoroutine(chargeMoveCoroutine);
                chargeMoveCoroutine = null;
            }

            transform.position = wallPosition;

            // 元の位置に戻る
            //StartCoroutine(MoveTo(originalPosition, chargeSpeed * 0.1f));
            //isCharging = false; // 突進を終了
        }
        else if (collision.gameObject.CompareTag("Shield") && shieldController.IsReflecting())
        {
            // 突進のMoveTo()だけを止める
            if (chargeMoveCoroutine != null)
            {
                StopCoroutine(chargeMoveCoroutine);
                chargeMoveCoroutine = null;
            }

            bossHpSlider.value -= 10;
            bossAnim.SetTrigger(counter);

            // 攻撃を停止
            CancelInvoke("Attack");



            // 元の位置に戻る
            StartCoroutine(MoveTo(transform.position - transform.forward * 10f, chargeSpeed * 0.4f));
            StartCoroutine(WaitTime(3));
            isCharging = false; // 突進を終了
        }
    }

    IEnumerator WaitTime(int second){
        yield return new WaitForSeconds(second);
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
            attackPattern.AddRange(new List<int> { 5, 2, 6, 6, 6, 50, 7, 10, 2, 11, 8, 50, 50, 12, 13 }); // 2方向突進さかな(緑)⇒岩⇒木の葉さかな(橙)*3⇒突進⇒盾貫通さかな(紫)⇒ノーマルさかな+2方向突進さかな
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
