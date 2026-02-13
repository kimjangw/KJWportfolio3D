using UnityEngine;
using Unity.Cinemachine; 

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    [Header("Cameras")]
    public CinemachineCamera freeLookCamera; 
    public CinemachineClearShot clearShotCamera; 

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