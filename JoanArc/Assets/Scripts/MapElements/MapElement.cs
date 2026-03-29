using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MapElement : MonoBehaviour
{
    [Header("Map Element Info")]
    public bool isMapElementThatCannotReuse;
    [Tooltip("Each map element's ID should be unique!")]
    public int mapElementID;

    protected virtual void Start()
    {
        DestroySelfIfHasBeenUsed();
    }

    private void DestroySelfIfHasBeenUsed()
    {
        if (isMapElementThatCannotReuse)
        {
            foreach (var id in GameManager.instance.UsedMapElementIDList)
            {
                if (mapElementID == id)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
