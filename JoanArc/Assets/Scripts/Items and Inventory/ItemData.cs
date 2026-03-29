using System.Text;
using UnityEngine;


#if UNITY_EDITOR
using UnityEditor;
#endif

public enum ItemType
{
    Material,
    Equipment
}

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Item")]
public class ItemData : ScriptableObject
{
    public ItemType itemType;
    public string itemName;
    public string itemName_Chinese;
    public Sprite icon;
    public string itemID;

    [Range(0, 100)]
    public float dropChance;

    protected StringBuilder sb = new StringBuilder();

    private void OnValidate()
    {
#if UNITY_EDITOR  //meaning the code inside this # (macro) will only be executed in unity editor, and won't be compiled when building the game
        string path = AssetDatabase.GetAssetPath(this);
        itemID = AssetDatabase.AssetPathToGUID(path);  //GUID means global unique identifier for the asset
#endif
    }

    public virtual string GetItemStatInfoAndEffectDescription()
    {
        return "";
    }
}
