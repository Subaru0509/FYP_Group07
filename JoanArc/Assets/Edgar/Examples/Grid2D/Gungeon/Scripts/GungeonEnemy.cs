using System;
using UnityEngine;

namespace Edgar.Unity.Examples.Gungeon
{
    public class GungeonEnemy : MonoBehaviour
    {
        public GungeonRoomManager RoomManager;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                RoomManager.OnEnemyKilled(this);
            }
        }
    }
}