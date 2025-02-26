using System.Collections;
using UnityEngine;
using UnityEngine.UI; // Textを使用する場合
using TMPro; // TextMeshProを使用する場合

public class Countdown : MonoBehaviour
{
    public Text countdownText; // UIのTextコンポーネント
    // public TMP_Text countdownText; // TextMeshProを使う場合はこちらを使用

    void Start()
    {
        StartCoroutine(CountdownRoutine());
    }

    IEnumerator CountdownRoutine()
    {
        string[] countdownStrings = { "","3", "2", "1", "START" };

        foreach (string text in countdownStrings)
        {
            countdownText.text = text;
            yield return new WaitForSeconds(1f);
        }
        

        countdownText.text = ""; // 最後に空にする
    }
}
