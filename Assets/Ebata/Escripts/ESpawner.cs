using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ESpawner : MonoBehaviour
{
    public GameObject PkeySpawn; // Pキーで生成するプレハブ
    public GameObject OkeySpawn; // Oキーで生成するプレハブ

    void Update()
    {
        // Pキーを押したときにプレハブを生成
        if (Input.GetKeyDown(KeyCode.P))
        {
            SpawnPrefab(PkeySpawn, Vector3.zero); // 生成位置を (0, 0, 0) に設定
        }
        // Oキーを押したときにプレハブを生成
        if (Input.GetKeyDown(KeyCode.O))
        {
            SpawnPrefab(OkeySpawn, Vector3.zero); // 生成位置を (0, 0, 0) に設定
        }

    }

    private void SpawnPrefab(GameObject prefab, Vector3 spawnPosition)
    {
        if (prefab != null)
        {
            // プレハブを生成
            Instantiate(prefab, spawnPosition, Quaternion.identity);
            Debug.Log($"Prefab {prefab.name} を {spawnPosition} に生成しました。");
        }
        else
        {
            Debug.LogError("生成するPrefabが設定されていません。");
        }
    }
}
