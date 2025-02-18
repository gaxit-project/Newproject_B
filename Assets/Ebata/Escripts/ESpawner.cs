using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ESpawner : MonoBehaviour
{
    public GameObject PkeySpawn; // Pキーで生成するプレハブ
    public GameObject OkeySpawn; // Oキーで生成するプレハブ
    public GameObject LkeySpawnA; //Lキーで生成するプレハブその1
    public GameObject LkeySpawnB; //Lキーで生成するプレハブその2
    public GameObject IkeySpawn; //Iキーで生成するプレハブ
    public GameObject KkeySpawn; //Kキーで生成するプレハブ
    public GameObject JkeySpawnA; //Jキーで生成するプレハブその1
    public GameObject JkeySpawnB; //Jキーで生成するプレハブその2



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
        //Lキーを押したときにプレハブを生成
        if (Input.GetKeyDown(KeyCode.L))
        {
            SpawnPrefab(LkeySpawnA, Vector3.zero); // 生成位置を (0, 0, 0) に設定
            SpawnPrefab(LkeySpawnB, Vector3.zero); // 生成位置を (0, 0, 0) に設定
        }
        //Iキーを押したときにプレハブを生成
        if (Input.GetKeyDown(KeyCode.I))
        {
            SpawnPrefab(IkeySpawn, Vector3.zero); // 生成位置を (0, 0, 0) に設定
        }
        //Kキーを押したときにプレハブを生成
        if (Input.GetKeyDown(KeyCode.K))
        {
            SpawnPrefab(KkeySpawn, Vector3.zero); // 生成位置を (0, 0, 0) に設定
        }
        //Jキーを押したときにプレハブを生成
        if (Input.GetKeyDown(KeyCode.J))
        {
            SpawnPrefab(JkeySpawnA, Vector3.zero); // 生成位置を (0, 0, 0) に設定
            SpawnPrefab(JkeySpawnB, Vector3.zero); // 生成位置を (0, 0, 0) に設定
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
