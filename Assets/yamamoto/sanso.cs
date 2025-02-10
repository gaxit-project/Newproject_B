using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // シーン管理のためのライブラリ

public class TimeDecreasingGauge : MonoBehaviour
{
    public Slider gaugeSlider; // スライダーUI
    public float maxValue = 100f; // ゲージの最大値
    public float decreaseRate = 5f; // 時間経過による減少速度（1秒あたりの減少量）

    private float currentValue; // 現在の値

    private void Start()
    {
        currentValue = maxValue; // 初期化
        gaugeSlider.maxValue = maxValue; // スライダーの最大値を設定
        gaugeSlider.value = currentValue; // スライダーの初期値を設定
    }

    private void Update()
    {
        // 時間経過でゲージを減少させる
        currentValue -= decreaseRate * Time.deltaTime;
        currentValue = Mathf.Clamp(currentValue, 0, maxValue); // 値を範囲内に制限

        // スライダーに値を反映
        gaugeSlider.value = currentValue;

        // ゲージが0になったときの処理
        if (currentValue <= 0)
        {
            OnGaugeDepleted();
        }
    }

    // ゲージが空になったときの処理
    private void OnGaugeDepleted()
    {
        Debug.Log("ゲージが空になりました！ ゲームオーバーシーンに遷移します。");
        SceneManager.LoadScene("GameOverScene"); // "GameOverScene" はゲームオーバーシーンの名前
    }
}
