using UnityEngine;

// 이 스크립트를 넣으면 AudioSource 컴포넌트가 자동으로 같이 붙습니다!
[RequireComponent(typeof(AudioSource))]
public class PlayerFootSound : MonoBehaviour
{
    private AudioSource audioSource;

    [Header("발소리 오디오 파일들")]
    public AudioClip[] walkClips;      // 걷기 소리만 넣는 곳
    public AudioClip[] runClips;       // 뛰기 소리만 넣는 곳
    public AudioClip[] jumpStartClips; // 점프 뛸 때(기합 등) 소리만 넣는 곳
    public AudioClip[] landClips;      // 착지할 때(쿵!) 소리만 넣는 곳

    [Header("볼륨 설정")]
    [Range(0f, 1f)] public float volume = 0.5f;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        // 시작할 때 불필요한 재생 막기 및 완벽한 3D 사운드 세팅
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1f;
    }

    // 걷기 애니메이션(Walk)에서 마커로 호출할 함수
    public void PlayWalkEvent()
    {
        PlayRandomClip(walkClips);
    }

    // 뛰기 애니메이션(Run)에서 마커로 호출할 함수
    public void PlayRunEvent()
    {
        PlayRandomClip(runClips);
    }

    // 점프(Jump) 시작 애니메이션에서 마커로 호출할 함수
    public void PlayJumpEvent()
    {
        PlayRandomClip(jumpStartClips);
    }

    // 착지(Land) 애니메이션에서 마커로 호출할 함수
    public void PlayLandEvent()
    {
        PlayRandomClip(landClips);
    }

    //  내부에서만 쓰는 재생 전용 헬퍼 함수 (코드 중복 방지)
    private void PlayRandomClip(AudioClip[] clips)
    {
        // 배열이 비어있으면 에러 없이 스무스하게 넘어감
        if (clips == null || clips.Length == 0) return;

        // 무작위로 하나 뽑기
        int randomIndex = Random.Range(0, clips.Length);
        AudioClip clip = clips[randomIndex];

        // 피치(음의 높낮이)를 미세하게 흔들어 자연스럽게 만들기
        audioSource.pitch = Random.Range(0.85f, 1.1f);

        // 소리 재생
        audioSource.PlayOneShot(clip, volume);
    }
}