using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class LanguageManager : MonoBehaviour, ISettingsSaveManager
{
    public static LanguageManager instance;

    //0 for english, 1 for chinese
    public int localeID { get; set; }

    public Dictionary<string, string> EnglishToChineseKeybindsDictionary;
    public Dictionary<string, string> EnglishToChineseEquipmentTypeDictionary;
    public Dictionary<string, string> EnglishToChineseStatDictionary;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        EnglishToChineseKeybindsDictionary = new Dictionary<string, string>();
        EnglishToChineseEquipmentTypeDictionary = new Dictionary<string, string>();
        EnglishToChineseStatDictionary = new Dictionary<string, string>();

        SetupEnglishToChineseKeybindsDictionary();
        SetupEnglishToChineseEquipmentTypeDictionary();
        SetupEnglishToChineseStatDictionary();
    }

    public void SetTextLanguageByLocaleID(int _localeID)
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[_localeID];
    }

    private void SetupEnglishToChineseKeybindsDictionary()
    {
        EnglishToChineseKeybindsDictionary.Add("Attack", "攻击");
        EnglishToChineseKeybindsDictionary.Add("Aim", "瞄准");
        EnglishToChineseKeybindsDictionary.Add("Flask", "元素瓶");
        EnglishToChineseKeybindsDictionary.Add("Dash", "冲刺");
        EnglishToChineseKeybindsDictionary.Add("Parry", "弹反");
        EnglishToChineseKeybindsDictionary.Add("Crystal", "水晶");
        EnglishToChineseKeybindsDictionary.Add("Blackhole", "黑洞");
        EnglishToChineseKeybindsDictionary.Add("Character", "角色面板");
        EnglishToChineseKeybindsDictionary.Add("Craft", "制造面板");
        EnglishToChineseKeybindsDictionary.Add("Skill", "技能面板");
    }

    private void SetupEnglishToChineseEquipmentTypeDictionary()
    {
        EnglishToChineseEquipmentTypeDictionary.Add("Weapon", "武器");
        EnglishToChineseEquipmentTypeDictionary.Add("Armor", "护甲");
        EnglishToChineseEquipmentTypeDictionary.Add("Charm", "护身符");
        EnglishToChineseEquipmentTypeDictionary.Add("Flask", "元素瓶");
    }

    private void SetupEnglishToChineseStatDictionary()
    {
        EnglishToChineseStatDictionary.Add("Strength", "力量");
        EnglishToChineseStatDictionary.Add("Agility", "敏捷");
        EnglishToChineseStatDictionary.Add("Intelligence", "智力");
        EnglishToChineseStatDictionary.Add("Vitality", "活力");
        EnglishToChineseStatDictionary.Add("Damage", "伤害");
        EnglishToChineseStatDictionary.Add("Crit Chance", "暴击率");
        EnglishToChineseStatDictionary.Add("Crit Power", "暴击伤害");
        EnglishToChineseStatDictionary.Add("Max HP", "最大生命值");
        EnglishToChineseStatDictionary.Add("Evasion", "闪避");
        EnglishToChineseStatDictionary.Add("Armor", "护甲");
        EnglishToChineseStatDictionary.Add("Magic Resist", "魔法抗性");
        EnglishToChineseStatDictionary.Add("Fire Dmg", "火焰伤害");
        EnglishToChineseStatDictionary.Add("Ice Dmg", "寒冰伤害");
        EnglishToChineseStatDictionary.Add("Lightning Dmg", "雷电伤害");
    }

    public string GetEnglishNameByChinese(string _chineseName)
    {
        string result = string.Empty;

        foreach (var search in EnglishToChineseKeybindsDictionary)
        {
            if (search.Value == _chineseName)
            {
                result = search.Key;
            }
        }

        return result;
    }

    public string TranslateItemStatInfoFromEnglishToChinese(string _itemStatInfo)
    {
        foreach (var search in EnglishToChineseStatDictionary)
        {
            string stat_English = search.Key;

            _itemStatInfo = _itemStatInfo.Replace(stat_English, EnglishToChineseStatDictionary[stat_English]);
        }

        return _itemStatInfo;
    }

    public void LoadData(SettingsData _data)
    {
        localeID = _data.localeID;

        SetTextLanguageByLocaleID(localeID);
    }

    public void SaveData(ref SettingsData _data)
    {
        _data.localeID = localeID;
    }
}
