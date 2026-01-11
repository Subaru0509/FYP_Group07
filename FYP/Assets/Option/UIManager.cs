using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private Transform potionUIParent;
    [SerializeField] private GameObject potionIconPrefab;

    private void Awake()
    {
        Instance = this;
    }

    public void AddPotionIcon()
    {
        Instantiate(potionIconPrefab, potionUIParent);
    }

    public bool UsePotion()
    {
        if (potionUIParent.childCount > 0)
        {
            Transform lastPotion = potionUIParent.GetChild(potionUIParent.childCount - 1);
            Destroy(lastPotion.gameObject);
            return true;
        }
        return false;
    }
}
