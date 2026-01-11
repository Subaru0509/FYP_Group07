using UnityEngine;

namespace Assets.PixelFantasy.PixelMonsters.Common.Scripts
{
    public class MonsterGrid : MonoBehaviour
    {
        public float CellSize;

        public void OnValidate()
        {
            for (var i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).transform.localPosition = new Vector3(i * CellSize - (transform.childCount - 1) / 2f * CellSize, 0, 0);
            }
        }
    }
}