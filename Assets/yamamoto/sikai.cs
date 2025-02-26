using UnityEngine;
using UnityEngine.UI;

public class HPWarningUI : MonoBehaviour
{
    public PlayerMovement PlayerMovement;  // HPを参照するスクリプト
    public int thresholdHP = 50;  // 画像を表示するHPの閾値
    public GameObject warningImage;  // 表示する画像

    void Start()
    {
        if (warningImage != null)
        {
            warningImage.SetActive(false);  // 最初は非表示
        }
    }

    void Update()
    {
        if (PlayerMovement != null && warningImage != null)
        {
            // HPが閾値を下回ったら画像を表示
            warningImage.SetActive(PlayerMovement.playerHP < thresholdHP);
        }
    }
}
