using UnityEngine;

// 이 스크립트를 넣으면 AudioSource 컴포넌트가 자동으로 같이 붙습니다!
[RequireComponent(typeof(AudioSource))]
public class PlayerFootSound : MonoBehaviour
{
    private AudioSource audioSource;

    [Header("발소리 오디오 파일들")]
    public AudioClip[] footstepClips;

    [Header("볼륨 설정")]
    [Range(0f, 1f)] public float volume = 0.5f;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        // 시작할 때 불필요한 재생 막기 및 3D 사운드 세팅
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1f; // 1이면 거리에 따라 소리가 작아지는 완벽한 3D 사운드가 됩니다.
    }

    // 애니메이션 타임라인에서 이 이름을 직접 호출할 겁니다! 
    public void PlayFootstepEvent()
    {
        // 배열에 파일이 없으면 에러가 나지 않게 방어
        if (footstepClips.Length == 0) return;

        // 여러 발소리 중 하나를 무작위로 선택
        int randomIndex = Random.Range(0, footstepClips.Length);
        AudioClip clip = footstepClips[randomIndex];

        // ⭐ 실무 꿀팁: 피치(음의 높낮이)를 미세하게 흔들어주면 
        // 발소리가 2개뿐이어도 매번 다른 소리처럼 자연스럽게 들립니다.
        audioSource.pitch = Random.Range(0.85f, 1.1f);

        // 소리 재생 (PlayOneShot을 써야 양발 소리가 겹쳐도 뚝뚝 끊기지 않습니다)
        audioSource.PlayOneShot(clip, volume);
    }
}