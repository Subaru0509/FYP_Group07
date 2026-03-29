using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownablePlatform : MonoBehaviour
{
    Collider2D cd;

    private void Start()
    {
        cd = GetComponent<Collider2D>();
    }

    public void TurnOffPlatformColliderForTime(float _seconds)
    {
        StartCoroutine(TurnOffPlatformColliderForTime_Coroutine(_seconds));
    }

    private IEnumerator TurnOffPlatformColliderForTime_Coroutine(float _seconds)
    {
        cd.enabled = false;
        yield return new WaitForSeconds(_seconds);
        cd.enabled = true;
    }

}
