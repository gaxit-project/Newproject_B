using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float acceleration = 2f;   // ������
    public float deceleration = 1f;   // ������
    public float maxSpeed = 5f;       // �ő呬�x
    public float turnSpeed = 3f;      // ��]���x�i�����]���݂̓��j

    private Vector3 currentVelocity = Vector3.zero; // ���݂̑��x�x�N�g��

    void Update()
    {
        // ���͂��擾
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Vector3 inputDirection = new Vector3(moveX, 0, moveZ).normalized;

        // ���͂�����ꍇ�͉����A�Ȃ��ꍇ�͌���
        if (inputDirection.magnitude > 0)
        {
            // ���݂̑��x����͕����Ɍ����ĉ���
            currentVelocity = Vector3.Lerp(currentVelocity, inputDirection * maxSpeed, acceleration * Time.deltaTime);
        }
        else
        {
            // ���݂̑��x������
            currentVelocity = Vector3.Lerp(currentVelocity, Vector3.zero, deceleration * Time.deltaTime);
        }

        // �v���C���[�̌�����ύX�i���X�Ɍ��������킹��j
        if (currentVelocity.magnitude > 0.1f) // �����ȑ��x�ł͌�����ς��Ȃ�
        {
            Quaternion targetRotation = Quaternion.LookRotation(currentVelocity);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }

        // �v���C���[���ړ�
        transform.Translate(currentVelocity * Time.deltaTime, Space.World);
    }
}
