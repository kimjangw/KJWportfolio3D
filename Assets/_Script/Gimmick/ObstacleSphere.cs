using UnityEngine;

public class ObstacleCollision : MonoBehaviour
{
    // 물리적 충돌이 발생했을 때 (통과하는 공은 이 함수가 아예 발동하지 않음!)
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();

            if (player != null)
            {
                // 이미 통과할 공들은 물리 엔진이 다 걸러주었으므로, 
                // 부딪힌 공은 무조건 피격(볼륨 효과)을 실행합니다!
                player.TakeHit();
            }
        }
    }
}