using UnityEngine;
using UnityEngine.UI;

public class DisableSlider : MonoBehaviour
{
    public Slider sliderbosshp;
    public Slider sliderplayerhp;
    public Slider slidersansohp;

    void Start()
    {
        // プレイヤーがスライダーを触れないようにする
        sliderbosshp.interactable = false;
        sliderplayerhp.interactable = false;
        slidersansohp.interactable = false;
    }
}
