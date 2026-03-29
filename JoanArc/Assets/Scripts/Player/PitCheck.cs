using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitCheck : MonoBehaviour
{
    private Player player;
    private CinemachineVirtualCamera cm;

    private Transform followTarget;

    private void Start()
    {
        player = PlayerManager.instance.player;

        cm = CameraManager.instance.cm;
        followTarget = player.transform;
    }

    private void Update()
    {
        cm.Follow = followTarget;

        CameraMovementOnPit();
    }

    private void CameraMovementOnPit()
    {
        if (player.isNearPit)
        {
            if (cm.m_Lens.OrthographicSize < CameraManager.instance.targetCameraLensSize)
            {
                cm.m_Lens.OrthographicSize = Mathf.Lerp(cm.m_Lens.OrthographicSize, CameraManager.instance.targetCameraLensSize, CameraManager.instance.cameraLensSizeChangeSpeed * Time.deltaTime);

                if (cm.m_Lens.OrthographicSize >= CameraManager.instance.targetCameraLensSize - 0.01f)
                {
                    cm.m_Lens.OrthographicSize = CameraManager.instance.targetCameraLensSize;
                }
            }
        }
        else
        {
            //can't let this influence camera movement on downable platform
            if (player.isOnPlatform)
            {
                return;
            }

            if (cm.m_Lens.OrthographicSize > CameraManager.instance.defaultCameraLensSize)
            {
                cm.m_Lens.OrthographicSize = Mathf.Lerp(cm.m_Lens.OrthographicSize, CameraManager.instance.defaultCameraLensSize, CameraManager.instance.cameraLensSizeChangeSpeed * Time.deltaTime);

                if (cm.m_Lens.OrthographicSize <= CameraManager.instance.defaultCameraLensSize + 0.01f)
                {
                    cm.m_Lens.OrthographicSize = CameraManager.instance.defaultCameraLensSize;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Pit>())
        {
            player.isNearPit = true;

            followTarget = collision.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Pit>())
        {
            player.isNearPit = false;

            followTarget = player.transform;
        }
    }
}
