using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ESpawner : MonoBehaviour
{
    [SerializeField] private GameObject FishManager;


    void Update()
    {
        // Pキーを押したときにプレハブを生成
        if (Input.GetKeyDown(KeyCode.P))
        {
            FishManager.SendMessage("spawnNormalFish");
        }
        // Oキーを押したときにプレハブを生成
        if (Input.GetKeyDown(KeyCode.O))
        {
            FishManager.SendMessage("spawnChaseFish");
        }
        //Lキーを押したときにプレハブを生成
        if (Input.GetKeyDown(KeyCode.L))
        {
            FishManager.SendMessage("spawnLeafFish");
        }
        //Iキーを押したときにプレハブを生成
        if (Input.GetKeyDown(KeyCode.I))
        {
            FishManager.SendMessage("spawnCoDFish");
        }
        //Jキーを押したときにプレハブを生成
        if (Input.GetKeyDown(KeyCode.J))
        {
            FishManager.SendMessage("spawnTwoWayPenetrateFish");
        }
    }
}
