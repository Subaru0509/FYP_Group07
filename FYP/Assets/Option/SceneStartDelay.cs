using System.Collections;
using UnityEngine;

public class SceneStartDelay : MonoBehaviour
{
    public Player player;

    void Start()
    {
        if (player != null)
        {
            player.enabled = false;
            StartCoroutine(EnablePlayerAfterDelay(2f));
        }
    }

    private IEnumerator EnablePlayerAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        player.enabled = true;
    }
}
