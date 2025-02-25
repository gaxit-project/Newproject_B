using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainBGMPitchController : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource; //ピッチを調整するAusioSourceを入れる
    [SerializeField] private Slider bossHP; //ボスHPスライダーを入れる
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
            Invoke("AccelerateBGM", 0);
        }
    }

    private void AccelerateBGM()
    {
        if(audioSource.pitch < 1.2f)
        {
            audioSource.pitch += 0.05f;
            Invoke("AccelerateBGM", 0.1f);
        }
    }
}
