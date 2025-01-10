using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpawner : MonoBehaviour
{
    public GameObject prefabToSpawn; // スペースキーで生成するプレハブ
    public GameObject alternatePrefabToSpawn; // Rキーで生成するプレハブ

    void Update()
    {
        // スペースキーを押したときにプレハブを生成
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnPrefab(prefabToSpawn, Vector3.zero); // 生成位置を (0, 0, 0) に設定
        }

        // Rキーを押したときに別のプレハブを生成
        if (Input.GetKeyDown(KeyCode.R))
        {
            SpawnPrefab(alternatePrefabToSpawn, new Vector3(2f, 0f, 0f)); // 生成位置を少しずらして設定
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
