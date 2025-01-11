using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


// "AudioSource"コンポーネントがアタッチされていない場合アタッチ
public class AudioManagerSE : MonoBehaviour
{

    [SerializeField] Slider slider;

    [SerializeField] AudioSource audioSource;

    public void Start()
    {
        audioSource.volume = PlayerPrefs.GetFloat("VolumeSE");
        slider.value = PlayerPrefs.GetFloat("VolumeSE");
    }
    public void SoundSliderOnValueChange(float newSliderValue)
    {
        audioSource.volume = newSliderValue;
        PlayerPrefs.SetFloat("VolumeSE", newSliderValue);
        PlayerPrefs.Save();
    }
}