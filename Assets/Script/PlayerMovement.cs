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

    void Start()
    {
        animator = GetComponent<Animator>(); // Animator を取得
        playerRenderer = GetComponentInChildren<Renderer>(); // Renderer を取得
    }

    void Update()
    {
        // 回避中は移動を無効化
        if (isDodging)
        {
            return;
        }

        // 入力を取得
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Vector3 inputDirection = new Vector3(moveX, 0, moveZ).normalized;

        // 入力がある場合は加速、ない場合は減速
        if (inputDirection.magnitude > 0)
        {
            currentVelocity = Vector3.Lerp(currentVelocity, inputDirection * maxSpeed, acceleration * Time.deltaTime);

            // プレイヤーを進行方向に向ける
            Quaternion targetRotation = Quaternion.LookRotation(inputDirection, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 10f * Time.deltaTime); // 10fは回転速度
        }
        else
        {
            currentVelocity = Vector3.Lerp(currentVelocity, Vector3.zero, deceleration * Time.deltaTime);
        }

        // アニメーションパラメーターを更新
        animator.SetFloat("Speed", currentVelocity.magnitude);

        // プレイヤーを移動
        transform.Translate(currentVelocity * Time.deltaTime, Space.World);

        // 回避処理
        if (Input.GetKeyDown(KeyCode.E))
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
        yield return new WaitForSeconds(2f); // 回避モーションの長さに応じて調整

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
            // プレイヤーのゲームオーバー処理をここに記述
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

}
