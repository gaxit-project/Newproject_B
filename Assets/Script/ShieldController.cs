using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : MonoBehaviour
{
    public List<GameObject> shieldPrefabs; // 盾のプレハブリスト
    public List<int> shieldHP; // 各盾のHP
    public Transform shieldSpawnPoint; // 盾を生成する位置
    public Transform playerTransform; // プレイヤーのTransform
    public float reflectDuration = 2f; // 反射モードの継続時間
    public float disableDuration = 2f; // 盾が非アクティブになる時間

    private GameObject currentShield; // 現在の盾
    private Renderer shieldRenderer;
    private int currentShieldIndex = 0; // 現在の盾のインデックス
    private int currentShieldHP; // 現在の盾のHP
    private bool isReflecting = false; // 反射モード中かどうかのフラグ

    void Start()
    {
        if (shieldPrefabs.Count > 0)
        {
            ValidateShieldHPList(); // HPリストの検証
            SpawnShield(); // 最初の盾を生成
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
            LogInfo($"盾HPリストが不足していたため、デフォルト値(5)を補いました。");
        }
        else if (shieldHP.Count > shieldPrefabs.Count)
        {
            shieldHP = shieldHP.GetRange(0, shieldPrefabs.Count); // 過剰分を削除
            LogInfo($"盾HPリストが多すぎたため、プレハブリストと一致するように調整しました。");
        }

        for (int i = 0; i < shieldHP.Count; i++)
        {
            LogInfo($"盾 {i + 1} のHP: {shieldHP[i]}");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isReflecting)
        {
            StartCoroutine(ReflectCoroutine()); // 反射モードを開始
        }
    }

    private IEnumerator ReflectCoroutine()
    {
        isReflecting = true;

        ChangeShieldColor(Color.green);
        LogInfo("反射モードが開始されました。");

        yield return new WaitForSeconds(reflectDuration);

        SetShieldActive(false);
        LogInfo("盾が一時的に無効化されました。");

        yield return new WaitForSeconds(disableDuration);

        SetShieldActive(true);
        ChangeShieldColor(Color.blue);
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

        LogInfo($"盾 {currentShieldIndex + 1} がダメージを受けました。残りHP: {currentShieldHP}");

        if (currentShieldHP <= 0)
        {
            LogInfo($"盾 {currentShieldIndex + 1} が破壊されました。次の盾に切り替えます。");
            ReplaceShield();
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

        // プレイヤーの向きを盾に適用
        Quaternion shieldRotation = playerTransform.rotation;

        // 現在の盾を生成し、プレイヤーの向きに合わせる
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
        }
    }

    private void ChangeShieldColor(Color color)
    {
        if (shieldRenderer != null)
        {
            shieldRenderer.material.color = color;
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
}
