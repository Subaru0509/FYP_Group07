using UnityEngine;

public class SaveRestore : MonoBehaviour
{
    private Player player;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void Start()
    {
        SaveData data = SaveManager.Load();
        if (data == null) return;

        player.transform.position = data.playerPosition;

        var health = player.GetComponent<Entity_Health>();
        if (health != null)
        {
            health.SetHealth(data.playerHP);
        }

        UIManager.Instance.SetPotionCount(data.potionCount);
    }
}
