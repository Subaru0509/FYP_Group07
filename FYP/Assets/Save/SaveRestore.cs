using UnityEngine;
using System.Collections;

public class SaveRestore : MonoBehaviour
{
    private IEnumerator Start()
    {
        yield return new WaitUntil(() => UIManager.Instance != null);

        SaveData data = SaveManager.Load();
        if (data == null) yield break;

        transform.position = data.playerPosition;

        var health = GetComponent<Entity_Health>();
        if (health != null)
            health.SetHealth(data.playerHP);

        UIManager.Instance.SetPotionCount(data.potionCount);
    }
}
