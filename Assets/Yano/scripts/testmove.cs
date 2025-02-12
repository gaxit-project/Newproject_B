using UnityEngine;

public class testmove : MonoBehaviour
{
    public float speed = 5f; // 移動速度

    void Update()
    {
        float moveX = Input.GetAxis("Horizontal"); // A, D キー
        float moveZ = Input.GetAxis("Vertical");   // W, S キー

        Vector3 move = new Vector3(moveX, 0f, moveZ) * speed * Time.deltaTime;
        transform.Translate(move, Space.World);
    }
}
