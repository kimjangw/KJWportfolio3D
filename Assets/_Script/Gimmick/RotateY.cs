using UnityEngine;

public class RotateY : MonoBehaviour
{
    [Header("회전 설정")]
    [Tooltip("초당 회전할 각도 (양수는 시계 방향, 음수는 반시계 방향)")]
    public float rotationSpeed = 90f;

    void Update()
    {
        // 실무 필수 최적화/안전장치: 부모가 실제로 존재하는지 먼저 검사합니다.
        if (transform != null)
        {
            // 부모의 Transform에 접근하여 Y축(Vector3.up) 기준으로 회전시킵니다.
            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        }
        else
        {
            // 만약 하이어라키에서 실수로 부모 밖으로 꺼내졌을 경우를 대비한 경고
            Debug.LogWarning($"{gameObject.name}에 부모 객체가 없어 회전할 수 없습니다!");
        }
    }
}