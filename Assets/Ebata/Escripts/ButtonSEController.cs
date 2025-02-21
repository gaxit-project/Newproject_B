using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class ButtonSEController : MonoBehaviour
{
    [SerializeField] AudioClip normal; //スタート、やりなおす以外のボタンを押したときになる音
    [SerializeField] AudioClip start; //スタート、やりなおすボタンを押したときになる音
    [SerializeField] AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource.volume = PlayerPrefs.GetFloat("VolumeSE"); //音量を保存された値に変える
    }

    public void normalButton()
    {
        audioSource.PlayOneShot(normal);
    }
    public void startButton()
    {
        audioSource.PlayOneShot(start);
    }
}
