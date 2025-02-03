using UnityEngine;
using UnityEngine.UI;

public class PLHPBar : MonoBehaviour
{
    public Slider hpSlider; // スライダーの参照
    public PlayerMovement  PlayerMovement; // PlayerMovementスクリプトの参照

    private void Start()
    {
        if (PlayerMovement != null)
        {
            hpSlider.maxValue = 10;
            hpSlider.value = PlayerMovement.playerHP;
        }
    }

    private void Update()
    {
        if (PlayerMovement != null)
        {
            hpSlider.value = PlayerMovement.playerHP; // HPに基づいてスライダーを更新
        }
    }
}
