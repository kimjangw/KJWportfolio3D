using UnityEngine;
using Unity.Cinemachine; // Unity 6 필수 네임스페이스

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    [Header("Cameras")]
    public CinemachineCamera freeLookCamera;  // 인스펙터에서 FreeLook 할당
    public CinemachineClearShot clearShotCamera; // 인스펙터에서 ClearShot 할당

    [Header("Priority Settings")]
    public int activePriority = 20;
    public int inactivePriority = 10;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void EnableClearShot(bool enable, PlayerController player = null)
    {
        if (enable)
        {
            clearShotCamera.Priority = activePriority;
            freeLookCamera.Priority = inactivePriority;
            if (player != null) player.SetCameraZoneMode(true);
        }
        else
        {
            clearShotCamera.Priority = inactivePriority;
            freeLookCamera.Priority = activePriority;
            if (player != null) player.SetCameraZoneMode(false);
        }
    }
}