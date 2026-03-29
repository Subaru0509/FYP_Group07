using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownablePlatformCheck : MonoBehaviour
{
    private Player player;

    private void Start()
    {
        player = PlayerManager.instance.player;
    }

    private void Update()
    {
        CameraManager.instance.CameraMovementOnDownablePlatform();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<DownablePlatform>() != null)
        {
            player.lastPlatform = collision.gameObject.GetComponent<DownablePlatform>();
            player.isOnPlatform = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<DownablePlatform>() != null)
        {
            player.lastPlatform = collision.gameObject.GetComponent<DownablePlatform>();
            player.isOnPlatform = false;
        }
    }
}
