using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace Edgar.Unity.Examples.Gungeon
{
    #region codeBlock:2d_gungeon_roomManager

    public class GungeonRoomManager : MonoBehaviour
    {
        #region hide

        /// <summary>
        /// Whether the room was cleared from all the enemies.
        /// </summary>
        public bool Cleared;

        /// <summary>
        /// Whether the room was visited by the player.
        /// </summary>
        public bool Visited;

        /// <summary>
        /// Doors of neighboring corridors.
        /// </summary>
        public List<GameObject> Doors = new List<GameObject>();

        #endregion

        /// <summary>
        /// Enemies that can spawn inside the room.
        /// </summary>
        [FormerlySerializedAs("Enemies")]
        public GameObject[] EnemyPrefabs;
        
        /// <summary>
        /// Enemies that are still alive in the room. (Do not change manually)
        /// </summary>
        public List<GungeonEnemy> RemainingEnemies;

        /// <summary>
        /// Whether enemies were spawned.
        /// </summary>
        public bool EnemiesSpawned;

        /// <summary>
        /// Collider of the floor tilemap layer.
        /// </summary>
        public Collider2D FloorCollider;
        
        /// <summary>
        /// Use the shared Random instance so that the results are properly seeded.
        /// </summary>
        private static System.Random Random => GungeonGameManager.Instance.Random;

        #region hide

        /// <summary>
        /// Room instance of the corresponding room.
        /// </summary>
        private RoomInstanceGrid2D roomInstance;

        /// <summary>
        /// Room info.
        /// </summary>
        private GungeonRoom room;
        
        public void Start()
        {
            roomInstance = GetComponent<RoomInfoGrid2D>()?.RoomInstance;
            room = roomInstance?.Room as GungeonRoom;
        }

        public void OnRoomEnter(GameObject player)
        {
            GungeonGameManager.Instance.OnRoomEnter(roomInstance);

            if (!Visited && roomInstance != null)
            {
                Visited = true;
                UnlockDoors();
            }

            if (ShouldSpawnEnemies())
            {
                // Close all neighboring doors
                CloseDoors();

                // Spawn enemies
                SpawnEnemies();
            }
        }

        public void OnRoomLeave(GameObject player)
        {
            if (GungeonGameManager.Instance)
            {
                GungeonGameManager.Instance.OnRoomLeave(roomInstance);
            }
        }

        #endregion

        private void SpawnEnemies()
        {
            EnemiesSpawned = true;
            
            var enemies = new List<GungeonEnemy>();
            var totalEnemiesCount = Random.Next(4, 8);

            while (enemies.Count < totalEnemiesCount)
            {
                // Find random position inside floor collider bounds
                var position = RandomPointInBounds(FloorCollider.bounds, 1f);

                // Check if the point is actually inside the collider as there may be holes in the floor, etc.
                if (!IsPointWithinCollider(FloorCollider, position))
                {
                    continue;
                }

                // We want to make sure that there is no other collider in the radius of 1
                if (Physics2D.OverlapCircleAll(position, 0.5f).Any(x => !x.isTrigger))
                {
                    continue;
                }

                // Pick random enemy prefab
                var enemyPrefab = EnemyPrefabs[Random.Next(0, EnemyPrefabs.Length)];

                // Create an instance of the enemy and set position and parent
                var enemy = Instantiate(enemyPrefab, roomInstance.RoomTemplateInstance.transform, true);
                enemy.transform.position = position;
                
                // Add the GungeonEnemy component to know when the enemy is killed
                var gungeonEnemy = enemy.AddComponent<GungeonEnemy>();
                gungeonEnemy.RoomManager = this;
                
                enemies.Add(gungeonEnemy);
            }

            // Store the list of all spawned enemies for tracking purposes
            RemainingEnemies = enemies;
        }

        private static bool IsPointWithinCollider(Collider2D collider, Vector2 point)
        {
            return collider.OverlapPoint(point);
        }

        private static Vector3 RandomPointInBounds(Bounds bounds, float margin = 0)
        {
            return new Vector3(
                RandomRange(bounds.min.x + margin, bounds.max.x - margin),
                RandomRange(bounds.min.y + margin, bounds.max.y - margin),
                RandomRange(bounds.min.z + margin, bounds.max.z - margin)
            );
        }

        private static float RandomRange(float min, float max)
        {
            return (float)(Random.NextDouble() * (max - min) + min);
        }

        #region hide
        
        /// <summary>
        /// Close doors before we spawn enemies.
        /// </summary>
        private void CloseDoors()
        {
            foreach (var door in Doors)
            {
                if (door.GetComponent<GungeonDoor>().State == GungeonDoor.DoorState.EnemyLocked)
                {
                    door.SetActive(true);
                }
            }
        }

        /// <summary>
        /// Open doors that were closed because of enemies.
        /// </summary>
        private void OpenDoors()
        {
            foreach (var door in Doors)
            {
                if (door.GetComponent<GungeonDoor>().State == GungeonDoor.DoorState.EnemyLocked)
                {
                    door.SetActive(false);
                }
            }
        }

        /// <summary>
        /// Unlock doors that were locked because there is a shop or reward room on the other end.
        /// </summary>
        private void UnlockDoors()
        {
            if (room.Type == GungeonRoomType.Reward || room.Type == GungeonRoomType.Shop)
            {
                foreach (var door in Doors)
                {
                    if (door.GetComponent<GungeonDoor>().State == GungeonDoor.DoorState.Locked)
                    {
                        door.GetComponent<GungeonDoor>().State = GungeonDoor.DoorState.Unlocked;
                    }
                }
            }
        }

        /// <summary>
        /// Check if we should spawn enemies based on the current state of the room and the type of the room.
        /// </summary>
        /// <returns></returns>
        private bool ShouldSpawnEnemies()
        {
            return Cleared == false && EnemiesSpawned == false && (room.Type == GungeonRoomType.Normal || room.Type == GungeonRoomType.Hub || room.Type == GungeonRoomType.Boss);
        }

        /// <summary>
        /// Called by an enemy when it is killed.
        /// Opens doors once there are no enemies left.
        /// </summary>
        /// <param name="enemy"></param>
        public void OnEnemyKilled(GungeonEnemy enemy)
        {
            Destroy(enemy.gameObject);
            RemainingEnemies.Remove(enemy);
            
            // Open doors if there are no enemies left in the room
            if (RemainingEnemies.Count == 0)
            {
                OpenDoors();
            }
        }

        #endregion
    }

    #endregion
}