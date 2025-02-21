using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : MonoBehaviour
{
    public List<GameObject> shieldPrefabs; // 盾のプレハブリスト
    public List<int> shieldHP; // 各盾のHP（上限）
    public Transform shieldSpawnPoint; // 盾を生成する位置
    public Transform playerTransform; // プレイヤーのTransform
    public float reflectDuration = 2f; // 反射モードの継続時間
    public float disableDuration = 2f; // 盾が非アクティブになる時間
    public float hpRecoveryInterval = 5f; // HP回復の間隔
    public int hpRecoveryAmount = 1; // 回復するHPの量

    private GameObject currentShield; // 現在の盾
    private Renderer shieldRenderer;
    private int currentShieldIndex = 0; // 現在の盾のインデックス
    private int currentShieldHP; // 現在の盾のHP
    private bool isReflecting = false; // 反射モード中かどうかのフラグ

    private bool isBossDamageCooldown = false; // ボス攻撃無効フラグ
    private float bossDamageCooldownDuration = 3f; // 無効時間

    public float bossShieldCollisionCooldown = 2f; // 2秒以内にボスがシールドと衝突していなければプレイヤーがダメージを受ける
    private float lastBossShieldCollisionTime = -10f; // ボスが最後にシールドと衝突した時間

    // **ボスとシールドの衝突時間を更新**
    public void RegisterBossShieldCollision()
    {
        lastBossShieldCollisionTime = Time.time;
    }

    // **ボスの最後の衝突時間を取得**
    public float GetLastBossShieldCollisionTime()
    {
        return lastBossShieldCollisionTime;
    }


    void Start()
    {
        if (shieldPrefabs.Count > 0)
        {
            ValidateShieldHPList(); // HPリストの検証
            SpawnShield(); // 最初の盾を生成
            StartCoroutine(RecoverShieldHP()); // HP回復開始
        }
        else
        {
            LogError("盾のプレハブリストが空です。");
        }
    }

    private void ValidateShieldHPList()
    {
        if (shieldHP.Count < shieldPrefabs.Count)
        {
            int difference = shieldPrefabs.Count - shieldHP.Count;
            for (int i = 0; i < difference; i++)
            {
                shieldHP.Add(5); // 不足分をデフォルトHP(5)で補う
            }
            LogInfo("盾HPリストが不足していたため、デフォルト値(5)を補いました。");
        }
        else if (shieldHP.Count > shieldPrefabs.Count)
        {
            shieldHP = shieldHP.GetRange(0, shieldPrefabs.Count); // 過剰分を削除
            LogInfo("盾HPリストが多すぎたため、プレハブリストと一致するように調整しました。");
        }

        for (int i = 0; i < shieldHP.Count; i++)
        {
            LogInfo($"盾 {i + 1} のHP: {shieldHP[i]}");
        }
    }

    private IEnumerator RecoverShieldHP()
{
    while (true)
    {
        yield return new WaitForSeconds(hpRecoveryInterval);

        if (currentShieldHP < shieldHP[currentShieldIndex] || currentShieldIndex > 0)
        {
            currentShieldHP += hpRecoveryAmount;
            LogInfo($"盾 {currentShieldIndex + 1} のHPが {hpRecoveryAmount} 回復しました。現在のHP: {currentShieldHP}");

            if (currentShieldHP > shieldHP[currentShieldIndex])
            {
                int excessHP = currentShieldHP - shieldHP[currentShieldIndex];
                currentShieldHP = shieldHP[currentShieldIndex];

                if (currentShieldIndex > 0)
                {
                    DestroyCurrentShield(); // 現在のシールドを破棄
                    currentShieldIndex--; // 前のシールドに切り替え
                    SpawnShield(); // 新しいシールドを生成
                    currentShieldHP = 1; // 戻ったシールドのHPを1に設定
                    //前のたてに戻るSE
                    SoundSE.RepairShield();
                    LogInfo($"盾 {currentShieldIndex + 1} に切り替わりました。現在の盾のHPは {currentShieldHP} です。");
                }
            }
        }
    }
}


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown("joystick button 0")&& !isReflecting)
        {
            StartCoroutine(ReflectCoroutine());
        }
    }

    private IEnumerator ReflectCoroutine()
    {
        isReflecting = true;

        ChangeShieldColor(Color.red); // 反射モードの色を赤に変更
        //ここに構える音
        SoundSE.StartReflection();
        LogInfo("反射モードが開始されました。");

        yield return new WaitForSeconds(reflectDuration);

        SetShieldActive(false);
        //ここに盾消滅音
        SoundSE.BrakeReflection();
        LogInfo("盾が一時的に無効化されました。");

        yield return new WaitForSeconds(disableDuration);

        SetShieldActive(true);
        ChangeShieldColor(Color.blue); // 通常時の色に戻す
        //ここに戻る音
        SoundSE.RepairReflection();
        LogInfo("盾が再有効化されました。");

        isReflecting = false;
    }


    public bool IsReflecting()
    {
        return isReflecting;
    }

    public void ReduceShieldHP()
    {
        if (isReflecting)
        {
            LogInfo("反射モード中のため、ダメージを無効化しました。");
            return;
        }

        currentShieldHP--;
        //被弾SE
        SoundSE.DamageShield();


        LogInfo($"盾 {currentShieldIndex + 1} がダメージを受けました。残りHP: {currentShieldHP}");

        if (currentShieldHP <= 0)
        {
            LogInfo($"盾 {currentShieldIndex + 1} が破壊されました。次の盾に切り替えます。");
            ReplaceShield();
            //壊れる盾SE
            SoundSE.BreakShield();

        }
    }

    public void ReplaceShield()
    {
        if (currentShieldIndex < shieldPrefabs.Count - 1)
        {
            DestroyCurrentShield();
            currentShieldIndex++;
            SpawnShield();
        }
        else
        {
            LogInfo("最後の盾です。壊れません！");
        }
    }

    private void SpawnShield()
    {
        if (playerTransform == null)
        {
            LogError("プレイヤーのTransformが設定されていません！");
            return;
        }

        // 盾の回転をプレイヤーの回転 + Y軸90度に設定
        Quaternion shieldRotation = playerTransform.rotation * Quaternion.Euler(0, 90, 0);

        currentShield = Instantiate(shieldPrefabs[currentShieldIndex], shieldSpawnPoint.position, shieldRotation, shieldSpawnPoint);
        currentShield.name = $"Shield_{currentShieldIndex}";

        shieldRenderer = currentShield.GetComponent<Renderer>();
        if (shieldRenderer == null) LogError($"Rendererが見つかりません: {currentShield.name}");

        ShieldCollision collisionHandler = currentShield.GetComponent<ShieldCollision>();
        if (collisionHandler != null)
        {
            collisionHandler.shieldController = this;
        }
        else
        {
            LogError($"ShieldCollisionが見つかりません: {currentShield.name}");
        }

        ChangeShieldColor(Color.blue);
        currentShieldHP = shieldHP[currentShieldIndex];

        LogInfo($"現在の盾 (Shield_{currentShieldIndex}) のHP: {currentShieldHP}");
    }


    private void DestroyCurrentShield()
{
    if (currentShield != null)
    {
        Destroy(currentShield);
        currentShield = null;
        LogInfo("現在の盾を破壊しました。");
    }
}

    private void ChangeShieldColor(Color color)
    {
        if (currentShield == null)
        {
            LogError("現在の盾が見つかりません。");
            return;
        }

        // 子オブジェクトの Renderer を取得
        Renderer[] childRenderers = currentShield.GetComponentsInChildren<Renderer>();

        if (childRenderers.Length == 0)
        {
            LogError("盾の子オブジェクトに Renderer が見つかりません。");
            return;
        }

        foreach (Renderer renderer in childRenderers)
        {
            if (renderer.material.HasProperty("_EmissionColor"))
            {
                // Emission（HDRカラー）を変更
                renderer.material.SetColor("_EmissionColor", color * 2.0f); // 明るさ調整
                renderer.material.EnableKeyword("_EMISSION"); // Emissionを有効化
            }
            else if (renderer.material.HasProperty("_BaseColor"))
            {
                // URP / HDRP の場合
                renderer.material.SetColor("_BaseColor", color);
            }
            else if (renderer.material.HasProperty("_Color"))
            {
                // Standard Shader の場合
                renderer.material.SetColor("_Color", color);
            }
            else
            {
                LogError("シェーダーに適切なカラープロパティが見つかりません。");
            }
        }
    }




    private void SetShieldActive(bool isActive)
    {
        if (currentShield != null)
        {
            currentShield.SetActive(isActive);
        }
    }

    private void LogError(string message)
    {
        Debug.LogError($"[ShieldController] {message}");
    }

    private void LogInfo(string message)
    {
        Debug.Log($"[ShieldController] {message}");
    }
    //Bossの突進関連
    public void ApplyBossDamage()
    {
        if (isBossDamageCooldown) // 無効時間中ならダメージを受けない
        {
            return;
        }

        currentShieldHP -= 5; // HPを5減少
        //Se
        SoundSE.ShieldBossDamage();

        LogInfo($"ボスの攻撃を受けた！盾 {currentShieldIndex + 1} のHPが 5 減少しました。現在のHP: {currentShieldHP}");

        if (currentShieldHP <= 0)
        {
            LogInfo($"盾 {currentShieldIndex + 1} が破壊されました。次の盾に切り替えます。");
            ReplaceShield();
        }

        // ボス攻撃のクールダウン開始
        StartCoroutine(BossDamageCooldown());
    }
    private IEnumerator BossDamageCooldown()
    {
        isBossDamageCooldown = true;
        LogInfo("ボスの攻撃が 3 秒間無効化されました！");
        yield return new WaitForSeconds(bossDamageCooldownDuration);
        isBossDamageCooldown = false;
        LogInfo("ボスの攻撃が再び有効になりました！");
    }

    public void ApplyKnockbackToPlayer(Vector3 bossPosition)
    {
        if (playerTransform == null) return;

        PlayerMovement playerMovement = playerTransform.GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            Vector3 knockbackDirection = (playerTransform.position - bossPosition).normalized; // ボスの反対方向
            float knockbackDistance = 20f; // ノックバックの距離
            float knockbackDuration = 2f; // ノックバックの時間

            playerMovement.ApplyKnockback(knockbackDirection, knockbackDistance, knockbackDuration);
        }

        LogInfo("プレイヤーがボスの攻撃でノックバックしました！");
    }

}
