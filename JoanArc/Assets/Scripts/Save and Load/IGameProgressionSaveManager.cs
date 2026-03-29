using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameProgressionSaveManager
{
    void LoadData(GameData _data);

    void SaveData(ref GameData _data);

    //void TestFunction()
    //{
    //    Debug.Log("cnm");
    //}
}
