using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class SoundSE : MonoBehaviour
{
    const int N = 100;
    public static AudioClip[] sound = new AudioClip[N];
    public AudioClip[] sound_se;

    public static AudioSource audioSource_tmp;
    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource_tmp = audioSource;
        sound[0] = sound_se[0];
        sound[1] = sound_se[1];
        sound[2] = sound_se[2];
        sound[3] = sound_se[3];
        sound[4] = sound_se[4];
        sound[5] = sound_se[5];
        sound[6] = sound_se[6];
        sound[7] = sound_se[7];
        sound[8] = sound_se[8];
        sound[9] = sound_se[9];
        sound[10] = sound_se[10];

    }

    public static void Button()
    {
        audioSource_tmp.PlayOneShot(sound[0]);
    }
    public static void Omake()
    {
        audioSource_tmp.PlayOneShot(sound[1]);
    }

    //èÇä÷òAÇÃSE
    public static void Reflect()
    {
        audioSource_tmp.PlayOneShot(sound[2]);
    }
    public static void RepairShield()
    {
        audioSource_tmp.PlayOneShot(sound[3]);
    }
    public static void DamageShield()
    {
        audioSource_tmp.PlayOneShot(sound[4]);
    }
    public static void BreakShield()
    {
        audioSource_tmp.PlayOneShot(sound[5]);
    }
    public static void StartReflection()
    {
        audioSource_tmp.PlayOneShot(sound[6]);
    }
    public static void BrakeReflection()
    {
        audioSource_tmp.PlayOneShot(sound[7]);
    }
    public static void RepairReflection()
    {
        audioSource_tmp.PlayOneShot(sound[8]);
    }
    public static void ShieldBossDamage()
    {
        audioSource_tmp.PlayOneShot(sound[9]);
    }

    //Playerä÷òAÇÃSE
    public static void PlayerDamage()
    {
        audioSource_tmp.PlayOneShot(sound[10]);
    }

    void Update(){
        
    }
}