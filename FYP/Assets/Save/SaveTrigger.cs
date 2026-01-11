using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveTrigger : MonoBehaviour
{
    [SerializeField] private Player player;

    public void SaveGame()
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
        Debug.Log("Save£¡");
    }
}
