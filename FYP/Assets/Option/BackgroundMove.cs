using UnityEngine;

public class BackgroundMove : MonoBehaviour
{
    [SerializeField] private float speed = 0.02f;
    [SerializeField] private float resetPositionX = -20f;
    [SerializeField] private float startPositionX = 20f;

    void Update()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;

        if (transform.position.x <= resetPositionX)
        {
            transform.position = new Vector3(startPositionX, transform.position.y, transform.position.z);
        }
    }
}
