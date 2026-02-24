using UnityEngine;
using UnityEngine.Playables;

public class BossRoomInputTrigger : MonoBehaviour
{
    [Header("재생할 타임라인 디렉터")]
    public PlayableDirector cutsceneDirector;

    // 컷신이 이미 재생되었는지 기억하는 스위치 (최초 1회 재생용)
    private bool hasPlayed = false;

    // 플레이어가 투명한 트리거 영역에 닿는 순간 자동으로 실행되는 함수
    private void OnTriggerEnter(Collider other)
    {
        // 1. 이미 재생된 적이 있거나
        // 2. 닿은 물체가 플레이어가 아니라면 무시하고 돌아갑니다.
        if (hasPlayed || !other.CompareTag("Player")) return;

        // 위에 걸러지지 않고 무사히 넘어왔다면? 컷신을 재생합니다!
        if (cutsceneDirector != null)
        {
            cutsceneDirector.Play(); // 타임라인 재생!
            hasPlayed = true;        // "나 이제 재생했음!" 하고 스위치를 잠가버립니다.
        }
    }
}