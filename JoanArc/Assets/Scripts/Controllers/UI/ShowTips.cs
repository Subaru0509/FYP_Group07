using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowTips : MonoBehaviour
{
    public GameObject Tips;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Tips.SetActive(true);

        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        // 确保只有玩家触发
        if (other.CompareTag("Player"))
        {


            Tips.SetActive(false);
        }
    }

    public void ShowehtTips()
    {
        Tips.SetActive(true);
    }

    public void CloseTips()
    {

        Tips.SetActive(false);
    }
}
