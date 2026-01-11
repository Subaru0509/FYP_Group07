using UnityEngine;

public class PotionPickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            UIManager.Instance.AddPotionIcon();
            Destroy(gameObject);
        }
    }
}
