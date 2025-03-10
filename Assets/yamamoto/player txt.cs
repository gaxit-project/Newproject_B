using UnityEngine;
using UnityEngine.UI;

public class BringTextToFront : MonoBehaviour
{
    public Text targetText; // 最前面にしたいテキスト

    void Start()
    {
        if (targetText != null)
        {
            targetText.transform.SetAsLastSibling();
        }
        else
        {
            Debug.LogError("targetTextが設定されていません");
        }
    }
}

