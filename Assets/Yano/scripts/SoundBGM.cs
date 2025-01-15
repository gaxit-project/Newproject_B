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

    // Start is called before the first frame update
    void Start()
    {
        audioSource_tmp = audioSource;
        sound[0] = sound_bgm[0];
        sound[1] = sound_bgm[1];
    }

    public static void Hidan()
    {
        audioSource_tmp.PlayOneShot(sound[0]);
    }
    public static void Omake()
    {
        audioSource_tmp.PlayOneShot(sound[1]);
    }
}