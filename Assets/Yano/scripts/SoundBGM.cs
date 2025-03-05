using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class SoundBGM : MonoBehaviour
{
    const int N = 100;
    public static AudioClip[] sound = new AudioClip[N];
    public AudioClip[] sound_bgm;

    public static AudioSource audioSource_tmp;
    public AudioSource audioSource;


    //[SerializeField] BossScript bossScript;

    // Start is called before the first frame update
    void Start()
    {
        audioSource_tmp = audioSource;
        sound[0] = sound_bgm[0];
        sound[1] = sound_bgm[1];
    }

    void Update()
    {
        /*if (bossScript.GetBossHP() <= 0)
        {
            audioSource.Stop();
        }*/
    }

    public static void MainBGM()
    {
        audioSource_tmp.clip = sound[0]; // �N���b�v��ݒ�
        audioSource_tmp.Play();          // �Đ�
    }

    public static void Omake()
    {
        audioSource_tmp.PlayOneShot(sound[1]);
    }
}