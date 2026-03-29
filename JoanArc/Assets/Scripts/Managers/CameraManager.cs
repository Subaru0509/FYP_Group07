using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    public CinemachineVirtualCamera cm;

    [Header("Camera Lens Info")]
    public float defaultCameraLensSize;
    public float targetCameraLensSize;
    public float cameraLensSizeChangeSpeed;

    [Header("Camera Screen Y Position Info")]
    public float defaultCameraYPosition;
    public float targetCameraYPosition;
    public float cameraYPositionChangeSpeed;

    //[Header("Camera Screen X Position Info")]
    //public float defaultCameraXPosition;
    //public float targetCameraXPositionOffset;
    //public float cameraXPositionChangeSpeed;

    private Player player;
    public CinemachineFramingTransposer ft { get; set; }


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

        ft = cm.GetCinemachineComponent<CinemachineFramingTransposer>();
    }

    private void Start()
    {
        player = PlayerManager.instance.player;
    }

    public void CameraMovementOnDownablePlatform()
    {
        //if player is on downable platform, camera lens size will increase
        //meanwhile camera y position will decrease
        if (player.isOnPlatform)
        {
            //m means mirroring the default unity camera
            if (cm.m_Lens.OrthographicSize < targetCameraLensSize)
            {
                cm.m_Lens.OrthographicSize = Mathf.Lerp(cm.m_Lens.OrthographicSize, targetCameraLensSize, cameraLensSizeChangeSpeed * Time.deltaTime);

                if (cm.m_Lens.OrthographicSize >= targetCameraLensSize - 0.01f)
                {
                    cm.m_Lens.OrthographicSize = targetCameraLensSize;
                }
            }

            if (ft.m_ScreenY > targetCameraYPosition)
            {
                ft.m_ScreenY = Mathf.Lerp(ft.m_ScreenY, targetCameraYPosition, cameraYPositionChangeSpeed * Time.deltaTime);

                if (ft.m_ScreenY >= targetCameraYPosition + 0.01f)
                {
                    ft.m_ScreenY = targetCameraYPosition;
                }
            }
        }
        //vice versa
        else
        {
            //can't let this influence camera movement on pit
            if (player.isNearPit)
            {
                return;
            }

            if (cm.m_Lens.OrthographicSize > defaultCameraLensSize)
            {
                cm.m_Lens.OrthographicSize = Mathf.Lerp(cm.m_Lens.OrthographicSize, defaultCameraLensSize, cameraLensSizeChangeSpeed * Time.deltaTime);

                if (cm.m_Lens.OrthographicSize <= defaultCameraLensSize + 0.01f)
                {
                    cm.m_Lens.OrthographicSize = defaultCameraLensSize;
                }
            }

            if (ft.m_ScreenY < defaultCameraYPosition)
            {
                ft.m_ScreenY = Mathf.Lerp(ft.m_ScreenY, defaultCameraYPosition, cameraYPositionChangeSpeed * Time.deltaTime);

                if (ft.m_ScreenY <= targetCameraYPosition - 0.01f)
                {
                    ft.m_ScreenY = targetCameraYPosition;
                }
            }
        }
    }
}
