using UnityEngine;

public class testcamera : MonoBehaviour
{
    public Transform target1;
    public Transform target2;
    public float smoothSpeed = 5f; // 補間速度
    public float minHeight = 5f; // 最小高さ（ズームイン）
    public float maxHeight = 15f; // 最大高さ（ズームアウト）
    public float zoomSpeed = 5f; // ズーム速度

    void LateUpdate()
    {
        if (target1 == null || target2 == null)
        {
            Debug.LogWarning("ターゲットが設定されていません");
            return;
        }

        // 2つのターゲットのX,Z座標の中間点を求める
        float targetX = (target1.position.x + target2.position.x) / 2f;
        float targetZ = (target1.position.z + target2.position.z) / 2f;
        
        // 2つのターゲット間の距離を計算
        float distance = Vector3.Distance(target1.position, target2.position);
        
        // 距離に応じたY座標の計算
        float targetHeight = Mathf.Clamp(distance, minHeight, maxHeight);
        
        // カメラの位置をスムーズに補間
        Vector3 newPosition = new Vector3(targetX, Mathf.Lerp(transform.position.y, targetHeight, Time.deltaTime * zoomSpeed), targetZ);
        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * smoothSpeed);
    }
}
