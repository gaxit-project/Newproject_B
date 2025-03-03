using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class MainBGMChangingController : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource; //ピッチを調整するAusioSourceを入れる
    [SerializeField] private Slider bossHP; //ボスHPスライダーを入れる
    [SerializeField] private AudioClip audioClip; //ボスHPが50以下になった時に変えるBGMを入れる
    private bool isCalled = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(bossHP.value <= 50 && !isCalled)
        {
            isCalled = true;
            Invoke("ChangeBGM", 0);
        }
    }

    private async void ChangeBGM()
    {
        if(audioSource.volume <= 0.001f)
        {
            audioSource.enabled = false;
            await Task.Delay(1000);
            audioSource.clip = audioClip;
            audioSource.volume = PlayerPrefs.GetFloat("VolumeBGM");
            audioSource.enabled = true;
        }
        else
        {
            audioSource.volume = audioSource.volume / 2f;
            Invoke("ChangeBGM", 0.1f);
        }
    }
}
