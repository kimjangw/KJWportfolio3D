using UnityEngine;

public class CameraZoneTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController pc = other.GetComponent<PlayerController>();
            CameraManager.Instance.EnableClearShot(true, pc);
            Debug.Log("구역 진입: 카메라 전환 및 이동 방향 고정");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController pc = other.GetComponent<PlayerController>();
            CameraManager.Instance.EnableClearShot(false, pc);
            Debug.Log("구역 이탈: 프리룩 복귀 및 자유 이동");
        }
    }
}