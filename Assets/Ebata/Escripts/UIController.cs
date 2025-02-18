using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public GameObject HiddenPanel; //ここに消したいパネルを入れる
    public GameObject DisplayedPanel; //ここに出したいパネルを入れる

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public async void HideAndDisplay()
    {
        await Task.Delay(500);
        HiddenPanel.SetActive(false);
        DisplayedPanel.SetActive(true);
    }
}
