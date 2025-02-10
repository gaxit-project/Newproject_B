using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PLHPBardel : MonoBehaviour
{
    public Slider hpSliderdel; // スライダーの参照
    public PlayerMovement PlayerMovement; // PlayerMovementスクリプトの参照
    public float delayTime = 1f; // 遅延時間

    private void Start()
    {
        if (PlayerMovement != null)
        {
            hpSliderdel.maxValue = 10;
            hpSliderdel.value = PlayerMovement.playerHP;
            StartCoroutine(UpdateHPBarWithDelay());
        }
    }

    private void Update()
    {
        if (PlayerMovement != null)
        {
            // HPが変わった時に遅延して更新する
            if (hpSliderdel.value != PlayerMovement.playerHP)
            {
                StartCoroutine(UpdateHPBarWithDelay());
            }
        }
    }

    private IEnumerator UpdateHPBarWithDelay()
    {
        float targetHP = PlayerMovement.playerHP; // 目標のHP
        float currentHP = hpSliderdel.value; // 現在のHP
        float elapsedTime = 0f; // 経過時間

        // 遅延時間内にスライダーを補間で動かす
        while (elapsedTime < delayTime)
        {
            hpSliderdel.value = Mathf.Lerp(currentHP, targetHP, elapsedTime / delayTime);
            elapsedTime += Time.deltaTime; // 経過時間を増加させる
            yield return null; // 次のフレームまで待機
        }

        // 最終的に目標HPに設定
        hpSliderdel.value = targetHP;
    }
}
