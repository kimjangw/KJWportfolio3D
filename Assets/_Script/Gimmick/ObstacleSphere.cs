using UnityEngine;

public class ObstacleCollision : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // PlayerHitEffect 대신 유저님의 'PlayerController'를 가져옵니다!
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();

            if (player != null)
            {
                player.TakeHit(); // PlayerController 안에 만든 함수 실행
            }
        }
    }
}