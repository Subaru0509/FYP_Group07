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

    public int GetPotionCount()
    {
        return potionUIParent.childCount;
    }

    public void SetPotionCount(int count)
    {
        for (int i = potionUIParent.childCount - 1; i >= 0; i--)
            Destroy(potionUIParent.GetChild(i).gameObject);

        for (int i = 0; i < count; i++)
            AddPotionIcon();
    }
}
