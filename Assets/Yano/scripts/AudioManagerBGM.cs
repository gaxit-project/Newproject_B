using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


// "AudioSource"コンポーネントがアタッチされていない場合アタッチ

public class AudioManagerBGM : MonoBehaviour
{

    [SerializeField] Slider slider;

    [SerializeField] AudioSource audioSource;

    public void Start()
    {
        audioSource.volume = PlayerPrefs.GetFloat("VolumeBGM");
        slider.value = PlayerPrefs.GetFloat("VolumeBGM");
    }
    public void SoundSliderOnValueChange(float newSliderValue)
    {
        audioSource.volume = newSliderValue;
        PlayerPrefs.SetFloat("VolumeBGM", newSliderValue);
        PlayerPrefs.Save();
    }
}