using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionCollision : MonoBehaviour
{
    [Header("Next Scene Settings")]
    public string nextSceneName = "MainScene";
    public string targetTag = "FinishLine"; // 부딪힐 큐브의 태그를 여기에 입력

    // OnCollisionEnter 대신 CharacterController 전용 충돌 감지 함수를 사용합니다.
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // 부딪힌 오브젝트의 태그가 설정한 태그(예: Finish)와 같은지 확인
        if (hit.gameObject.CompareTag(targetTag))
        {
            Debug.Log($"[SceneTransition] '{targetTag}' 벽과 충돌! '{nextSceneName}' 씬 로드");
            SceneManager.LoadScene(nextSceneName);
        }
    }
}