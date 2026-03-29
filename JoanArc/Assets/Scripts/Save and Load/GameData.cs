using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.AI;

[System.Serializable]
public class GameData
{
    public int currecny;

    public SerializableDictionary<string, bool> skillTree;
    public SerializableDictionary<string, int> inventory;
    public List<string> equippedEquipmentIDs;

    public SerializableDictionary<string, bool> checkpointsDictionary;
    public string closestActivatedCheckpointID;
    public string lastActivatedCheckpointID;

    [Header("Dropped Currency")]
    public int droppedCurrencyAmount;
    public Vector2 deathPosition;

    [Header("Map Elements")]
    public List<int> UsedMapElementIDList;

    public GameData()
    {
        //Game Progression
        this.currecny = 0;
        this.droppedCurrencyAmount = 0;
        this.deathPosition = Vector2.zero;

        //setup picked up item in map list, by default it should be empty
        UsedMapElementIDList = new List<int>();
        //pickedUpItemInMapList = new List<ItemObject>();

        //skillTree<skillName, unlocked>
        skillTree = new SerializableDictionary<string, bool>();
        //inventory<itemID, stackSize>
        inventory = new SerializableDictionary<string, int>();

        equippedEquipmentIDs = new List<string>();

        //checkpoints<checkpointID, activated>
        checkpointsDictionary = new SerializableDictionary<string, bool>();

        closestActivatedCheckpointID = string.Empty;
        lastActivatedCheckpointID = string.Empty;
    }
}
