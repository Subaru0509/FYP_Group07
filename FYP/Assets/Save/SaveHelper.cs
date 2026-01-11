using UnityEngine;
using UnityEngine.SceneManagement;

public static class SaveHelper
{
    public static void SaveGame(Player player)
    {
        if (player == null) return;

        Entity_Health health = player.GetComponent<Entity_Health>();
        if (health == null) return;

        SaveData data = new SaveData();
        data.sceneName = SceneManager.GetActiveScene().name;
        data.playerPosition = player.transform.position;
        data.playerHP = health.CurrentHP;
        data.potionCount = UIManager.Instance.GetPotionCount();

        SaveManager.Save(data);
    }
}
