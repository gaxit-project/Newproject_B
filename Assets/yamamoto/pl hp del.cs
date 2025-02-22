using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PLHPBardel : MonoBehaviour
{
    public Slider hpSliderdel;          // スライダーの参照
    public PlayerMovement PlayerMovement; // PlayerMovementスクリプトの参照
    public float delayTime = 1f;        // 補間時間
    public float waitTime = 1f;         // ダメージ後の待機時間

    private float targetHP;             // 目標のHP
    private Coroutine updateCoroutine;  // 現在動作中の補間コルーチン

    private void Start()
    {
        if (PlayerMovement != null)
        {
            hpSliderdel.maxValue = 10;
            hpSliderdel.value = PlayerMovement.playerHP;
            targetHP = PlayerMovement.playerHP;
        }
    }

    private void Update()
    {
        // HPに変化があった場合
        if (PlayerMovement != null && targetHP != PlayerMovement.playerHP)
        {
            targetHP = PlayerMovement.playerHP;
            // 既存の補間処理があれば停止
            if (updateCoroutine != null)
            {
                StopCoroutine(updateCoroutine);
            }
            updateCoroutine = StartCoroutine(WaitAndUpdateHP());
        }
    }

    private IEnumerator WaitAndUpdateHP()
    {
        // まずwaitTime秒待機（ダメージ後の待機）
        yield return new WaitForSeconds(waitTime);

        float startHP = hpSliderdel.value;
        float elapsedTime = 0f;

        // delayTime秒かけてHPを補間する
        while (elapsedTime < delayTime)
        {
            hpSliderdel.value = Mathf.Lerp(startHP, targetHP, elapsedTime / delayTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        hpSliderdel.value = targetHP;
    }
}
