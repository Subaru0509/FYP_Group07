using UnityEngine;

public class InstantKillTrigger : MonoBehaviour
{
    [SerializeField] private bool triggerOnce = true;

    private bool hasTriggered;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasTriggered && triggerOnce)
            return;

        if (!collision.CompareTag("Player"))
            return;

        Entity_Health playerHealth = collision.GetComponent<Entity_Health>();

        if (playerHealth == null)
            playerHealth = collision.GetComponentInParent<Entity_Health>();

        if (playerHealth == null)
            return;

        playerHealth.SetHealth(0f);
        hasTriggered = true;
    }
}