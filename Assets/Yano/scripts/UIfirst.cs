using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIfirst : MonoBehaviour
{
    public GameObject firstSelectedButton; // 最初に選択するUIボタン

    void Start()
    {
        // 最初のボタンを選択状態にする
        EventSystem.current.SetSelectedGameObject(firstSelectedButton);
    }
}
