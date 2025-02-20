using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishContoller : MonoBehaviour
{
    public GameObject Rubble; //岩のprefab
    public GameObject NormalFish; //普通のさかなのprefab
    public GameObject ChaseFish; //追跡するさかなのprefab
    public GameObject DashFish; //一定時間経過後突進するさかなのprefab
    public GameObject CoDFishA; //途中で方向転換するさかなのprefabその1
    public GameObject CoDFishB; //途中で方向転換するさかなのprefabその2
    public GameObject LeafFish; //葉っぱが舞うように動くさかなのprefab
    public GameObject PenetrateFish; //盾を貫通するさかなのprefabその1
    public GameObject PenetrateFishA; //盾を貫通するさかなのprefabその2
    public GameObject PenetrateFishB; //盾を貫通するさかなのprefabその3

    private GameObject spawnPoint; //ボスがいる位置
    private Vector3 spawnPosition; //prefabをスポーンさせる場所

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        spawnPoint = GameObject.FindWithTag("Boss"); //タグを用いてボスを探す
        //ボスの位置からprefabをスポーンさせる場所を決定する
        spawnPosition = spawnPoint.transform.position + spawnPoint.transform.forward * 10f; 

        //テスト用　岩をボスの位置にスポーンさせてボスの体力を減らすもの
        if(Input.GetKeyDown(KeyCode.K))
        {
            Instantiate(Rubble, spawnPoint.transform.position, spawnPoint.transform.rotation);
        }
    }

    //スポーンさせるprefabの種類と数を攻撃パターンごとに分ける
    public void spawnRubble() //岩
    {
        Instantiate(Rubble, spawnPosition, spawnPoint.transform.rotation); 
    }
    public void spawnNormalFish() //ノーマルさかな
    {
        Instantiate(NormalFish, spawnPosition, spawnPoint.transform.rotation);
    }
    public void spawnChaseFish() //追尾さかな
    {
        Instantiate(ChaseFish, spawnPosition, spawnPoint.transform.rotation);
    }
    public void spawnDashFish() //突進さかな
    {
        Instantiate(DashFish, spawnPosition, spawnPoint.transform.rotation);
    }
    public void spawnCoDFish() //2方向突進さかな
    {
        Instantiate(CoDFishA, spawnPosition, spawnPoint.transform.rotation);
        Instantiate(CoDFishB, spawnPosition, spawnPoint.transform.rotation);
    }
    public void spawnLeafFish() //木の葉さかな
    {
        for(int i = 0; i < 3; i++)
        {
            Instantiate(LeafFish, spawnPosition, spawnPoint.transform.rotation);
        }
    }
    public void spawnPenetrateFish() //盾貫通さかな
    {
        Instantiate(PenetrateFish, spawnPosition, spawnPoint.transform.rotation);
    }
    public void spawnTwoWayPenetrateFish() //2方向盾貫通さかな
    {
        Instantiate(PenetrateFishA, spawnPosition, spawnPoint.transform.rotation);
        Instantiate(PenetrateFishB, spawnPosition, spawnPoint.transform.rotation);
    }
}
