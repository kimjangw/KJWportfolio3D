using UnityEngine;
using UnityEngine.Playables; // 타임라인 제어를 위해 반드시 추가해야 합니다!

public class EndingTrigger : MonoBehaviour
{
    [Header("재생할 엔딩 타임라인")]
    public PlayableDirector endingCutscene;

    // 플레이어가 구역에 계속 머물거나 다시 들어왔을 때 
    // 컷신이 중복 재생되는 것을 막기 위한 안전장치입니다.
    private bool hasPlayed = false;

    // 콜라이더(Trigger) 영역 안에 무언가 들어왔을 때 자동으로 실행되는 함수
    private void OnTriggerEnter(Collider other)
    {
        // 1. 이미 컷신이 재생되었거나
        // 2. 들어온 오브젝트가 'Player' 태그가 아니라면 (공이나 몬스터 등) 무시!
        if (hasPlayed || !other.CompareTag("Player")) return;

        // 조건을 통과했다면 스위치를 잠그고 컷신을 재생합니다.
        hasPlayed = true;

        if (endingCutscene != null)
        {
            endingCutscene.Play();
            // 💡 팁: 타임라인이 시작되자마자 이전에 만든 'LockInputSignal'이 
            // 발동하도록 타임라인 맨 앞에 마커를 찍어두시면 플레이어가 멈춥니다!
        }
    }
}