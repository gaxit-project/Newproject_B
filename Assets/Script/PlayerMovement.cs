using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float acceleration = 2f;   // 加速力
    public float deceleration = 1f;   // 減速力
    public float maxSpeed = 5f;       // 最大速度
    public float turnSpeed = 3f;      // 回転速度（方向転換の鈍さ）

    private Vector3 currentVelocity = Vector3.zero; // 現在の速度ベクトル

    void Update()
    {
        // 入力を取得
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Vector3 inputDirection = new Vector3(moveX, 0, moveZ).normalized;

        // 入力がある場合は加速、ない場合は減速
        if (inputDirection.magnitude > 0)
        {
            // 現在の速度を入力方向に向けて加速
            currentVelocity = Vector3.Lerp(currentVelocity, inputDirection * maxSpeed, acceleration * Time.deltaTime);
        }
        else
        {
            // 現在の速度を減速
            currentVelocity = Vector3.Lerp(currentVelocity, Vector3.zero, deceleration * Time.deltaTime);
        }

        // プレイヤーの向きを変更（徐々に向きを合わせる）
        if (currentVelocity.magnitude > 0.1f) // 微小な速度では向きを変えない
        {
            Quaternion targetRotation = Quaternion.LookRotation(currentVelocity);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }

        // プレイヤーを移動
        transform.Translate(currentVelocity * Time.deltaTime, Space.World);
    }
}
