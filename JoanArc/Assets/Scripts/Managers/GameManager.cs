using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, IGameProgressionSaveManager
{
    public static GameManager instance;

    private Player player;

    [SerializeField] private Checkpoint[] checkpoints;
    public string lastActivatedCheckpointID { get; set; }

    [Header("Dropped Currency")]
    [SerializeField] private GameObject deathBodyPrefab;
    public int droppedCurrencyAmount;
    [SerializeField] private Vector2 deathPosition;

    //public List<ItemObject> pickedUpItemInMapList { get; set; }
    public List<int> UsedMapElementIDList {  get; set; }

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

        checkpoints = FindObjectsOfType<Checkpoint>();
        player = PlayerManager.instance.player;

        //pickedUpItemInMapList = new List<ItemObject>();
        UsedMapElementIDList = new List<int>();
    }

    public void RestartScene()
    {
        Scene scene = SceneManager.GetActiveScene();

        SceneManager.LoadScene(scene.name);
    }

    public void PauseGame(bool _pause)
    {
        if (_pause)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    private Checkpoint FindClosestActivatedCheckpoint()
    {
        float closestDistance = Mathf.Infinity;
        Checkpoint closestActivatedCheckpoint = null;

        foreach (var checkpoint in checkpoints)
        {
            float distanceToCheckpoint = Vector2.Distance(player.transform.position, checkpoint.transform.position);

            if (distanceToCheckpoint < closestDistance && checkpoint.activated == true)
            {
                closestDistance = distanceToCheckpoint;
                closestActivatedCheckpoint = checkpoint;
            }
        }

        return closestActivatedCheckpoint;
    }

    private void LoadDroppedCurrency(GameData _data)
    {
        droppedCurrencyAmount = _data.droppedCurrencyAmount;
        deathPosition = _data.deathPosition;

        if (droppedCurrencyAmount > 0)
        {
            GameObject deathBody = Instantiate(deathBodyPrefab, deathPosition, Quaternion.identity);
            deathBody.GetComponent<DroppedCurrencyController>().droppedCurrency = droppedCurrencyAmount;
        }

        //to prevent from generating a death body on ground
        //when player is not dead in the last life with some currency left
        //and chose to save and continue game;
        droppedCurrencyAmount = 0;
    }

    private void LoadCheckpoints(GameData _data)
    {
        foreach (var search in _data.checkpointsDictionary)
        {
            foreach (var checkpoint in checkpoints)
            {
                if (checkpoint.checkpointID == search.Key && search.Value == true)
                {
                    checkpoint.ActivateCheckpoint();
                }
            }
        }
    }

    private void LoadLastActivatedCheckpoint(GameData _data)
    {
        lastActivatedCheckpointID = _data.lastActivatedCheckpointID;
    }

    private void SpawnPlayerAtClosestActivatedCheckpoint(GameData _data)
    {
        if (_data.closestActivatedCheckpointID == null)
        {
            return;
        }

        foreach (var checkpoint in checkpoints)
        {
            if (_data.closestActivatedCheckpointID == checkpoint.checkpointID)
            {
                player.transform.position = checkpoint.transform.position;
            }
        }
    }

    private void SpawnPlayerAtLastActivatedCheckpoint(GameData _data)
    {
        if (_data.lastActivatedCheckpointID == null)
        {
            return;
        }

        foreach (var checkpoint in checkpoints)
        {
            if (_data.lastActivatedCheckpointID == checkpoint.checkpointID)
            {
                player.transform.position = checkpoint.transform.position;
            }
        }
    }

    private void LoadPickedUpItemInMapIDList(GameData _data)
    {
        if (_data.UsedMapElementIDList != null)
        {
            foreach (var seach in _data.UsedMapElementIDList)
            {
                UsedMapElementIDList.Add(seach);
            }
        }
    }


    //private void LoadPickedUpItemInMapList(GameData _data)
    //{
    //    if (_data.pickedUpItemInMapList != null)
    //    {
    //        foreach (var item in _data.pickedUpItemInMapList)
    //        {
    //            pickedUpItemInMapList.Add(item);
    //        }
    //    }
    //}

    public void LoadData(GameData _data)
    {
        LoadDroppedCurrency(_data);

        //picked up item-in-map will get destoyed automatically in ItemObject script
        LoadPickedUpItemInMapIDList(_data);
        //LoadPickedUpItemInMapList(_data);

        //activate all the checkpoints which are saved as activated
        LoadCheckpoints(_data);

        LoadLastActivatedCheckpoint(_data);

        //player will be spawned at the closest activated checkpoint
        //SpawnPlayerAtClosestActivatedCheckpoint(_data);

        //player will be spawned at the last activated checkpoint
        SpawnPlayerAtLastActivatedCheckpoint(_data);
    }

    public void SaveData(ref GameData _data)
    {
        //saving death position and dropped currency
        _data.droppedCurrencyAmount = droppedCurrencyAmount;
        _data.deathPosition = player.transform.position;


        //prevent from saving duplicated redundant checkpoint data
        _data.checkpointsDictionary.Clear();

        _data.closestActivatedCheckpointID = FindClosestActivatedCheckpoint()?.checkpointID;

        foreach (Checkpoint checkpoint in checkpoints)
        {
            _data.checkpointsDictionary.Add(checkpoint.checkpointID, checkpoint.activated);
        }

        //save last activated checkpoint id
        _data.lastActivatedCheckpointID = lastActivatedCheckpointID;

        //save pciked up item in map list;
        _data.UsedMapElementIDList.Clear();
        foreach (var itemID in UsedMapElementIDList)
        {
            _data.UsedMapElementIDList.Add(itemID);
        }
    }
}
