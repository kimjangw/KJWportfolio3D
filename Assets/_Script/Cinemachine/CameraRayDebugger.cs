using UnityEngine;

[ExecuteInEditMode] // 에디터 모드에서도 보이게 함
public class CameraRayDebugger : MonoBehaviour
{
    public Transform cameraTransform; // 체크할 자식 카메라 (예: Left)
    public Transform targetTransform; // 플레이어 타겟 (CamTarget)
    public LayerMask obstacleLayer;   // 벽 레이어

    void Update()
    {
        if (cameraTransform == null || targetTransform == null) return;

        Vector3 start = cameraTransform.position;
        Vector3 end = targetTransform.position;
        Vector3 direction = end - start;
        float distance = direction.magnitude;

        // 레이캐스트 실행 (시네머신과 동일한 로직)
        if (Physics.Raycast(start, direction, out RaycastHit hit, distance, obstacleLayer))
        {
            // 벽에 막힘 -> 빨간색 선
            Debug.DrawLine(start, hit.point, Color.red);
            // 막힌 지점에 작은 구체 표시
            Debug.DrawRay(hit.point, Vector3.up * 0.5f, Color.red);

            // 콘솔에 어떤 오브젝트에 막혔는지 출력 (선택 사항)
            // Debug.Log($"막힘: {hit.collider.name}");
        }
        else
        {
            // 뚫려 있음 -> 초록색 선
            Debug.DrawLine(start, end, Color.green);
        }
    }
}