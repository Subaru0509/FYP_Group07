using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;

public class ItemToolTip_UI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemTypeText;
    [SerializeField] private TextMeshProUGUI itemStatInfo;

    [Range(1, 72)]
    [SerializeField] private int originalItemNameFontSize;

    private void Start()
    {
        itemNameText.fontSize = originalItemNameFontSize;
    }

    public void ShowToolTip(ItemData_Equipment item)
    {
        if (item == null)
        {
            return;
        }

        itemTypeText.text = item.equipmentType.ToString();
        itemStatInfo.text = item.GetItemStatInfoAndEffectDescription();

        if (LanguageManager.instance.localeID == 0)
        {
            itemNameText.text = item.itemName;
        }
        else if (LanguageManager.instance.localeID == 1)
        {
            itemNameText.text = item.itemName_Chinese;
            itemTypeText.text = LanguageManager.instance.EnglishToChineseEquipmentTypeDictionary[itemTypeText.text];
            itemStatInfo.text = LanguageManager.instance.TranslateItemStatInfoFromEnglishToChinese(itemStatInfo.text);
        }


        if (itemNameText.text.Length > 12)
        {
            itemNameText.fontSize *= 0.8f;
        }
        else
        {
            itemNameText.fontSize = originalItemNameFontSize;
        }

        gameObject.SetActive(true);

        //Debug.Log(itemNameText.text);
    }


    //public string TranslateItemStatInfoFromEnglishToChinese(string _itemStatInfo)
    //{
    //    foreach (var search in LanguageManager.instance.EnglishToChineseStatDictionary)
    //    {
    //        string stat_English = search.Key;

    //        _itemStatInfo = _itemStatInfo.Replace(stat_English, LanguageManager.instance.EnglishToChineseStatDictionary[stat_English]);
    //    }

    //    return _itemStatInfo;
    //}


    public void HideToolTip()
    {
        itemNameText.fontSize = originalItemNameFontSize;
        gameObject.SetActive(false);
    }
}
