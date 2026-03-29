using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaSound : MonoBehaviour
{
    [SerializeField] private int areaSoundIndex;

    private Coroutine stopSFXGradually;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            if (stopSFXGradually != null)
            {
                StopCoroutine(stopSFXGradually);
            }

            AudioManager.instance.sfx[areaSoundIndex].volume = 1;
            AudioManager.instance.PlaySFX(areaSoundIndex, null);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!gameObject.activeSelf)
        {
            return;
        }

        if (collision.GetComponent<Player>() != null)
        {
            //AudioManager.instance.StopSFXGradually(areaSoundIndex);
            stopSFXGradually = StartCoroutine(AudioManager.instance.DecreaseVolumeGradually(AudioManager.instance.sfx[areaSoundIndex]));
        }
    }
}
