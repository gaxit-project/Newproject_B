using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : MonoBehaviour
{
    public bool isReflecting = false;
    public float reflectDuration = 2f;
    public float disableDuration = 2f;

    private Renderer shieldRenderer;

    void Start()
    {
        shieldRenderer = GetComponent<Renderer>();
        SetShieldColor(Color.blue); // 初期状態: 青
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
        SetShieldColor(Color.green); // 反射中: 緑
        yield return new WaitForSeconds(reflectDuration);

        isReflecting = false;
        SetShieldColor(Color.red); // 無効化: 赤 ここは後で消滅に
        yield return new WaitForSeconds(disableDuration);

        SetShieldColor(Color.blue); // 待機: 青
    }

    private void SetShieldColor(Color color)
    {
        if (shieldRenderer != null)
        {
            shieldRenderer.material.color = color;
        }
    }
}
