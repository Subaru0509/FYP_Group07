using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Net.NetworkInformation;

//Save location:
//C:\Users\megum\AppData\LocalLow\DefaultCompany\BrandNewRPGGame_2nd


//***************************************************************************************
//Important: ****************************************************************************
//The save managers in both game scene and main menu should have the same encrypt option!
//***************************************************************************************

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    [SerializeField] private string settingsFileName;
    [SerializeField] private string gameFileName;
    [SerializeField] private bool encryptData;

    private GameData gameData;
    private SettingsData settingsData;

    
    private List<IGameProgressionSaveManager> gameProgressionSaveManagers;
    private FileDataHandler gameDataHandler;


    private List<ISettingsSaveManager> settingsSaveManagers;
    private FileDataHandler settingsDataHandler;

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
        settingsDataHandler = new FileDataHandler(Application.persistentDataPath, settingsFileName, encryptData);


        gameDataHandler = new FileDataHandler(Application.persistentDataPath, gameFileName, encryptData);

        settingsSaveManagers = FindAllSettingsSaveManagers();
        gameProgressionSaveManagers = FindAllGameProgressionSaveManagers();

        LoadGame();
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    public void NewGame()
    {
        gameData = new GameData();
    }

    public void LoadGame()
    {
        //Load Settings
        LoadSettings();

        //Load Game Progression
        LoadGameProgression();
    }

    private void LoadSettings()
    {
        settingsData = settingsDataHandler.LoadSettings();

        //default settings?
        if (settingsData == null)
        {
            settingsData = new SettingsData();
            Debug.Log("No settings data found, loading default settings");
        }

        foreach (var saveManager in settingsSaveManagers)
        {
            saveManager.LoadData(settingsData);
        }
    }

    private void LoadGameProgression()
    {
        // gameData = date from dataHandler
        gameData = gameDataHandler.LoadGameProgression();

        if (gameData == null)
        {
            Debug.Log("No save data found, entering new game");
            NewGame();
        }

        foreach (IGameProgressionSaveManager saveManager in gameProgressionSaveManagers)
        {
            saveManager.LoadData(gameData);
        }

        Debug.Log($"Loaded currency: {gameData.currecny}");
    }

    public void SaveGame()
    {
        //Save settings...
        SaveSettings();

        //Save Game Progression
        SaveGameProgression();
    }

    public void SaveSettings()
    {
        foreach (var saveManagers in settingsSaveManagers)
        {
            saveManagers.SaveData(ref settingsData);
        }

        settingsDataHandler.SaveSettings(settingsData);
    }

    private void SaveGameProgression()
    {
        foreach (IGameProgressionSaveManager saveManager in gameProgressionSaveManagers)
        {
            saveManager.SaveData(ref gameData);
        }

        gameDataHandler.SaveGameProgression(gameData);

        //Debug.Log($"Saved currency: {gameData.currecny}");
    }


    private List<ISettingsSaveManager> FindAllSettingsSaveManagers()
    {
        IEnumerable<ISettingsSaveManager> saveManagers = FindObjectsOfType<MonoBehaviour>().OfType<ISettingsSaveManager>();
        return new List<ISettingsSaveManager>(saveManagers);
    }

    private List<IGameProgressionSaveManager> FindAllGameProgressionSaveManagers()
    {
        IEnumerable<IGameProgressionSaveManager> saveManagers = FindObjectsOfType<MonoBehaviour>().OfType<IGameProgressionSaveManager>();

        //new List<ISaveManager>() here is just a constructor
        return new List<IGameProgressionSaveManager>(saveManagers); 
    }



    [ContextMenu("Delete game progression save file")]
    public void DeleteGameProgressionSavedData()
    {
        gameDataHandler = new FileDataHandler(Application.persistentDataPath, gameFileName, encryptData);
        gameDataHandler.DeleteSave();
    }

    public bool HasGameSaveData()
    {
        if (gameDataHandler.LoadGameProgression() != null)
        {
            return true;
        }

        return false;
    }

}