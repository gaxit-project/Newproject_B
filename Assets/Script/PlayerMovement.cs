using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public float acceleration = 5f;   // 加速力
    public float deceleration = 2f;   // 減速力
    public float maxSpeed = 10f;      // 最大速度
    public int playerHP = 10;         // プレイヤーのHP
    public float invincibilityDuration = 2f; // 無敵状態の継続時間
    public float blinkInterval = 0.1f; // 点滅間隔

    private Vector3 currentVelocity = Vector3.zero; // 現在の速度ベクトル
    private bool isDodging = false;                 // 回避中かどうかのフラグ
    private bool isInvincible = false;             // 無敵状態かどうかのフラグ

    private Animator animator;                      // Animator コンポーネント
    private Renderer playerRenderer;               // プレイヤーのRenderer
    private CharacterController characterController; // CharacterController 追加
    private ShieldController shieldController;

    private float fixedY; // 初期の Y 座標を保持

    private bool isKnockback = false; // ノックバック中かどうかのフラグ

    void Start()
    {
        animator = GetComponent<Animator>(); // Animator を取得
        playerRenderer = GetComponentInChildren<Renderer>(); // Renderer を取得
        characterController = GetComponent<CharacterController>(); // CharacterController を取得
        shieldController = FindObjectOfType<ShieldController>();

        // シールドの反射時間と同期
        if (shieldController != null)
        {
            invincibilityDuration = shieldController.reflectDuration; // 無敵時間を統一
        }

        // **初期 Y 座標を固定**
        fixedY = transform.position.y;
    }

    void Update()
    {
        if (isDodging || isKnockback) return; // 回避中 or ノックバック中は移動を無効化

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Vector3 inputDirection = new Vector3(moveX, 0, moveZ).normalized;

        // 通常移動処理
        if (inputDirection.magnitude > 0)
        {
            currentVelocity = Vector3.Lerp(currentVelocity, inputDirection * maxSpeed, acceleration * Time.deltaTime);
            Quaternion targetRotation = Quaternion.LookRotation(inputDirection, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
        }
        else
        {
            currentVelocity = Vector3.Lerp(currentVelocity, Vector3.zero, deceleration * Time.deltaTime);
        }

        // **Y 座標を固定して移動**
        Vector3 newPosition = transform.position + (currentVelocity * Time.deltaTime);
        newPosition.y = fixedY; // Y座標を初期値に固定

        characterController.Move(newPosition - transform.position); // 移動処理

        // アニメーションパラメーターを更新
        animator.SetFloat("Speed", currentVelocity.magnitude);

        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown("joystick button 0"))
        {
            StartCoroutine(Dodge());
        }
    }



    private IEnumerator Dodge()
    {
        isDodging = true; // 回避中フラグを有効化

        // 現在のアニメーションを強制終了し回避モーションを再生
        animator.Play("Dodge", 0, 0);

        // 回避中は一定時間停止
        float dodgeDuration = (shieldController != null) ? shieldController.reflectDuration : 2f;
        yield return new WaitForSeconds(dodgeDuration);

        isDodging = false; // 回避終了
    }

    public void TakeDamage()
    {
        if (isInvincible)
        {
            Debug.Log("無敵状態のためダメージを無効化しました。");
            return;
        }

        playerHP--;
        Debug.Log("Player HP: " + playerHP);

        if (playerHP <= 0)
        {
            Debug.Log("Game Over");
            SceneManager.LoadScene("Gameoverscene");
        }
        else
        {
            StartCoroutine(StartInvincibility()); // ダメージを受けた後に無敵状態を開始
        }
    }

    private IEnumerator StartInvincibility()
    {
        isInvincible = true;

        float elapsed = 0f;
        bool isVisible = true;

        while (elapsed < invincibilityDuration)
        {
            isVisible = !isVisible; // 表示/非表示を切り替え
            playerRenderer.enabled = isVisible; // レンダラーをオン/オフ
            yield return new WaitForSeconds(blinkInterval); // 点滅間隔
            elapsed += blinkInterval;
        }

        playerRenderer.enabled = true; // 最後に表示を確実にオンにする
        isInvincible = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Fish または Rubble タグを持つオブジェクトに当たった場合、HPを減らす
        if (other.CompareTag("Fish") || other.CompareTag("Rubble"))
        {
            Debug.Log(other.tag + " に当たりました！");
            TakeDamage(); // HPを減少させる
        }
    }
    public void ApplyKnockback(Vector3 direction, float distance, float duration)
    {
        if (isKnockback) return; // すでにノックバック中なら処理しない

        StartCoroutine(KnockbackCoroutine(direction, distance, duration));
    }

    private IEnumerator KnockbackCoroutine(Vector3 direction, float distance, float duration)
    {
        isKnockback = true; // ノックバック開始
        float elapsedTime = 0f;

        Vector3 knockbackVelocity = direction.normalized * (distance / duration); // ノックバック速度
        knockbackVelocity.y = 0; // Y軸の移動を防ぐ

        // **ノックバック中は敵の方向を向く**
        Quaternion knockbackRotation = Quaternion.LookRotation(-direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, knockbackRotation, 10f * Time.deltaTime);

        while (elapsedTime < duration)
        {
            characterController.Move(knockbackVelocity * Time.deltaTime); // **毎フレーム移動**
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        isKnockback = false; // ノックバック終了
    }




}
