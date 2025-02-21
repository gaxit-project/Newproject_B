using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMVolumeController : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource.volume = PlayerPrefs.GetFloat("VolumeBGM"); //音量を保存された値に変える
    }
}
