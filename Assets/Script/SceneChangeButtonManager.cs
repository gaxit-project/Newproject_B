using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeButtonManager : MonoBehaviour
{

    //ボタンが押されて指定秒数経過したか確認する
    private bool isButtonCoolingdown = false;
　　//ロードするシーンを選択できるようにする
    [SerializeField] private string _loadScene;
    private int _delayTime = 1; //かける遅延の長さ
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isButtonCoolingdown == true)
        {
            Invoke("CoolDown",2);
        }
    }

    public void CoolDown()
    {
         isButtonCoolingdown = false;
    }

    public void CheckCoolDown() //ボタンのクールダウンが完了しているか
    {
        if(isButtonCoolingdown == false)
        {
            isButtonCoolingdown = true;
            LagTime();
        }
    }

    public void LagTime() //シーン遷移に遅延をかける
    {
        Invoke("ChangeScene", _delayTime);
    }

    public void ChangeScene() //シーン遷移
    {
        SceneManager.LoadScene(_loadScene);
    }
}
