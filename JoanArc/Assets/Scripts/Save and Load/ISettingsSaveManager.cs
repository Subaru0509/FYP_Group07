using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISettingsSaveManager
{
    void LoadData(SettingsData _data);

    void SaveData(ref SettingsData _data);
}
