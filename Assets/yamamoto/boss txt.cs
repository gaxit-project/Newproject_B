using UnityEngine;
using UnityEngine.UI;

public class SliderTextColorChanger : MonoBehaviour
{
    public Slider targetSlider; // 監視するスライダー
    public Text targetText; // 色を変えるテキスト
    public float threshold = 30f; // しきい値

    private Color defaultColor;

    void Start()
    {
        if (targetText != null)
        {
            defaultColor = targetText.color; // 初期の色を記憶
        }

        if (targetSlider != null)
        {
            targetSlider.onValueChanged.AddListener(UpdateTextColor);
            UpdateTextColor(targetSlider.value); // 初期値チェック
        }
    }

    void UpdateTextColor(float value)
    {
        if (targetText != null)
        {
            targetText.color = value <= threshold ? Color.red : defaultColor;
        }
    }
}