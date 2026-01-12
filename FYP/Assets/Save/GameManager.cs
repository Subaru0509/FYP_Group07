using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public float playerHealth = 100f;
    public int playerPotions = 3;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
