using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class FishContoller : MonoBehaviour
{

    private GameObject spawnPoint; //ボスがいる位置
    private Vector3 spawnPosition; //prefabをスポーンさせる場所
    private Vector3 shiftedPosition; //prefabが生成される場所を変える用
    private int position = 0; //prefabが生成される場所を変える用
    [SerializeField] private GameObject Rubble; //岩のprefab
    [SerializeField] private GameObject NormalFish; //普通のさかなのprefab
    [SerializeField] private GameObject ChaseFish; //追跡するさかなのprefab
    [SerializeField] private GameObject DashFish; //一定時間経過後突進するさかなのprefab
    [SerializeField] private GameObject CoDFishA; //途中で方向転換するさかなのprefabその1
    [SerializeField] private GameObject CoDFishB; //途中で方向転換するさかなのprefabその2
    [SerializeField] private GameObject LeafFish; //葉っぱが舞うように動くさかなのprefab
    [SerializeField] private GameObject PenetrateFish; //盾を貫通するさかなのprefabその1
    [SerializeField] private GameObject PenetrateFishA; //盾を貫通するさかなのprefabその2
    [SerializeField] private GameObject PenetrateFishB; //盾を貫通するさかなのprefabその3
    [SerializeField] private GameObject Effect; //出現時に出すエフェクトを入れる
    [SerializeField] private Slider bossHP; //ボスHPスライダーを入れる

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(position == 0)
        {
            shiftedPosition = Vector3.zero;
        }
        else if(position == 1)
        {
            shiftedPosition = Vector3.right * 0.5f;
        }
        else if(position == 2)
        {
            shiftedPosition = Vector3.left * 0.5f;
        }
        else if(position == 3)
        {
            shiftedPosition = Vector3.right;
        }
        else if(position == 4)
        {
            shiftedPosition = Vector3.left;
        }

        spawnPoint = GameObject.FindWithTag("Boss"); //タグを用いてボスを探す
        //ボスの位置からprefabをスポーンさせる場所を決定する
        spawnPosition = spawnPoint.transform.position + spawnPoint.transform.forward * 10f + shiftedPosition; 

        //デバッグ用　直接ボスの体力を10減らす
        if(Input.GetKeyDown(KeyCode.K))
        {
            bossHP.value -= 10;    
        }
    }

    //スポーンさせるprefabの種類と数を攻撃パターンごとに分ける
    public void spawnRubble() //岩
    {
        SoundSE.Sound_SpawnRubble();
        Instantiate(Rubble, spawnPosition, spawnPoint.transform.rotation);
        Instantiate(Effect, spawnPosition, spawnPoint.transform.rotation);  
        Invoke("ChangePosition", 0);
    }
    public void spawnNormalFish() //ノーマルさかな
    {
        SoundSE.Sound_SpawnFish();
        Instantiate(NormalFish, spawnPosition, spawnPoint.transform.rotation);
        Instantiate(Effect, spawnPosition, spawnPoint.transform.rotation);
        Invoke("ChangePosition", 0);
    }
    public void spawnChaseFish() //追尾さかな
    {
        SoundSE.Sound_SpawnFish();
        Instantiate(ChaseFish, spawnPosition, spawnPoint.transform.rotation);
        Instantiate(Effect, spawnPosition, spawnPoint.transform.rotation);
        Invoke("ChangePosition", 0);
    }
    public void spawnDashFish() //突進さかな
    {
        SoundSE.Sound_SpawnFish();
        Instantiate(DashFish, spawnPosition, spawnPoint.transform.rotation);
        Instantiate(Effect, spawnPosition, spawnPoint.transform.rotation);
        Invoke("ChangePosition", 0);
    }
    public void spawnCoDFish() //2方向突進さかな
    {
        SoundSE.Sound_SpawnFish();
        Instantiate(CoDFishA, spawnPosition, spawnPoint.transform.rotation);
        Instantiate(Effect, spawnPosition, spawnPoint.transform.rotation);
        Invoke("ChangePosition", 0);
        Instantiate(CoDFishB, spawnPosition, spawnPoint.transform.rotation);
        Instantiate(Effect, spawnPosition, spawnPoint.transform.rotation);
        Invoke("ChangePosition", 0);
    }
    public async void spawnLeafFish() //木の葉さかな
    {
        for(int i = 0; i < 3; i++)
        {
            SoundSE.Sound_SpawnFish();
            Instantiate(LeafFish, spawnPosition+ spawnPoint.transform.forward * 10f, spawnPoint.transform.rotation);
            Instantiate(Effect, spawnPosition+ spawnPoint.transform.forward * 10f, spawnPoint.transform.rotation);
            await Task.Delay(800);
        }
    }
    public void spawnPenetrateFish() //盾貫通さかな
    {
        SoundSE.Sound_SpawnFish();
        Instantiate(PenetrateFish, new Vector3(spawnPosition.x, 10f, spawnPosition.z), spawnPoint.transform.rotation);
        Instantiate(Effect, spawnPosition, spawnPoint.transform.rotation);
        Invoke("ChangePosition", 0);
    }
    public void spawnTwoWayPenetrateFish() //2方向盾貫通さかな
    {
        SoundSE.Sound_SpawnFish();
        Instantiate(PenetrateFishA, new Vector3(spawnPosition.x, 10f, spawnPosition.z), spawnPoint.transform.rotation);
        Instantiate(Effect, spawnPosition, spawnPoint.transform.rotation);
        Invoke("ChangePosition", 0);
        Instantiate(PenetrateFishB, new Vector3(spawnPosition.x, 10f, spawnPosition.z), spawnPoint.transform.rotation);
        Instantiate(Effect, spawnPosition, spawnPoint.transform.rotation);
        if(position <= 3)
        Invoke("ChangePosition", 0);
    }
    public void ChangePosition()
    {
        if(position <= 3)
        {
            position++;
        }
        else
        {
            position = 0;
        }
    }
}
