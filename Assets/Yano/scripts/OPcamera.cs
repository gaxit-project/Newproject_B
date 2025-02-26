using UnityEngine;

public class MoveAndRotate : MonoBehaviour
{
    private Vector3 startPosition = new Vector3(0, 14.6f, -46.8f);
    private Vector3 targetPosition = new Vector3(0, 25.6f, -21.29f);
    private Quaternion startRotation = Quaternion.Euler(30, 0, 0);
    private Quaternion targetRotation = Quaternion.Euler(90, 0, 0);
    private float duration = 5f;
    private float elapsedTime = 0f;

    void Start()
    {
        transform.position = startPosition;
        transform.rotation = startRotation;
    }

    void Update()
    {
        if (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, t);
        }else{
            UnityEngine.SceneManagement.SceneManager.LoadScene("Mainscene");
        }
    }
}
