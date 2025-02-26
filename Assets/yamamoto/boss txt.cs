using UnityEngine;
using UnityEngine.UI;

public class BossHPWarning : MonoBehaviour
{
    public BossScript bossScript; // BossScript をアタッチ
    public Text warningText; // 変更するテキスト
    public float warningThreshold = 50f; // 50以下で赤くする

    private Color defaultColor; // 元のテキストの色を保存

    private void Start()
    {
        if (warningText != null)
        {
            defaultColor = warningText.color; // 初期カラーを保存
        }
    }

    private void Update()
    {
        if (bossScript != null && warningText != null)
        {
            if (bossScript.bossHpSlider.value <= warningThreshold)
            {
                warningText.color = Color.red; // HP が一定以下なら赤色
            }
            else
            {
                warningText.color = defaultColor; // それ以外は元の色
            }
        }
    }
}