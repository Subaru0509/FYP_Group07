using UnityEngine;

public class PortalTrigger : MonoBehaviour
{
    [SerializeField] private string targetScene = "Level2";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SceneTransitionManager.TransitionTo(targetScene);
        }
    }
}
