using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HPWarningUI : MonoBehaviour
{
    public PlayerMovement PlayerMovement;  // HPを参照するスクリプト
    public int thresholdHP = 50;  // 画像を表示するHPの閾値
    public GameObject warningImage;  // 表示する画像
    public float fadeDuration = 1f; // フェード時間

    private CanvasGroup canvasGroup;
    private bool hasFadedIn = false; // フェードインしたかどうか

    void Start()
    {
        if (warningImage != null)
        {
            // CanvasGroupがない場合は追加
            canvasGroup = warningImage.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = warningImage.AddComponent<CanvasGroup>();
            }

            canvasGroup.alpha = 0f;  // 最初は透明
            warningImage.SetActive(false);
        }
    }

    void Update()
    {
        if (PlayerMovement != null && warningImage != null)
        {
            if (PlayerMovement.playerHP <= thresholdHP && !hasFadedIn)
            {
                warningImage.SetActive(true);
                StartCoroutine(FadeIn());
            }
        }
    }

    IEnumerator FadeIn()
    {
        hasFadedIn = true;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 1f;
    }
}
