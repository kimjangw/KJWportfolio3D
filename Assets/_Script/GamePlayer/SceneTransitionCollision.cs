using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionCollision : MonoBehaviour
{
    [Header("Next Scene Settings")]
    public string nextSceneName = "GameScene";
    public string playerLayerName = "Player";

    private int playerLayerIndex;

    private void Awake()
    {
        playerLayerIndex = LayerMask.NameToLayer(playerLayerName);
    }

    // Is Trigger가 꺼져 있는 딱딱한 콜라이더끼리 부딪혔을 때 호출됩니다.
    private void OnCollisionEnter(Collision collision)
    {
        // 충돌한 오브젝트(collision.gameObject)의 레이어 확인
        if (collision.gameObject.layer == playerLayerIndex)
        {
            Debug.Log($"[SceneTransition] 벽에 부딪힘! '{nextSceneName}' 씬 로드");
            SceneManager.LoadScene(nextSceneName);
        }
    }
}