using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : MonoBehaviour
{
    public GameObject shield; // シールドオブジェクト
    public float reflectDuration = 2f;
    public float disableDuration = 2f;

    private Renderer shieldRenderer;
    private bool isReflecting = false;

    void Start()
    {
        if (shield != null)
        {
            shieldRenderer = shield.GetComponent<Renderer>();
            SetShieldColor(Color.blue); // 初期状態: 青
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isReflecting)
        {
            StartCoroutine(ReflectCoroutine());
        }
    }

    private IEnumerator ReflectCoroutine()
    {
        isReflecting = true;

        if (shield != null)
        {
            SetShieldColor(Color.green); // 反射中: 緑
        }

        yield return new WaitForSeconds(reflectDuration);

        isReflecting = false;

        if (shield != null)
        {
            shield.SetActive(false); // 非アクティブ化
        }

        yield return new WaitForSeconds(disableDuration);

        if (shield != null)
        {
            shield.SetActive(true); // 再アクティブ化
            SetShieldColor(Color.blue); // 待機: 青
        }
    }

    private void SetShieldColor(Color color)
    {
        if (shieldRenderer != null)
        {
            shieldRenderer.material.color = color;
        }
    }
}
