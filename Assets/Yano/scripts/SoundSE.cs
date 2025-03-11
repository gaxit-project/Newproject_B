using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class SoundSE : MonoBehaviour
{
    public static AudioClip[] sound;
    public AudioClip[] sound_se;

    public static AudioSource audioSource_tmp;
    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {

        audioSource_tmp = audioSource;
        sound = new AudioClip[sound_se.Length];


        for (int i = 0; i < sound_se.Length; i++)
        {
            sound[i] = sound_se[i];
        }
    }

    public static void Button()
    {
        audioSource_tmp.PlayOneShot(sound[0]);
    }
    public static void Omake()
    {
        audioSource_tmp.PlayOneShot(sound[1]);
    }

    //���֘A��SE
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

    //Player�֘A��SE
    public static void PlayerDamage()
    {
        audioSource_tmp.PlayOneShot(sound[10]);
    }

    public static void BossRockDamage()
    {
        audioSource_tmp.PlayOneShot(sound[11]);
    }

    public static void BossDashAttack()
    {
        audioSource_tmp.PlayOneShot(sound[12]);
    }

    public static void Sound_SpawnRubble()
    {
        audioSource_tmp.PlayOneShot(sound[13]);
    }
    
    public static void Sound_SpawnFish()
    {
        audioSource_tmp.PlayOneShot(sound[14]);
    }
}