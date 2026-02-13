using UnityEngine;

[ExecuteAlways] // 에디터에서 타임라인 바를 움직일 때 미리보기 가능하게 함
public class TimelineDissolveController : MonoBehaviour
{
    [Header("설정")]
    public string propertyName = "_DesolveValue"; // 셰이더 프로퍼티 이름 (정확해야 함)

    [Header("타임라인 제어 (0 ~ 1)")]
    [Range(0f, 1f)]
    public float dissolveAmount = 0f; // 타임라인이 건드릴 변수

    // 내부 변수
    private Renderer[] _allRenderers;
    private MaterialPropertyBlock _propBlock;
    private int _propertyId;

    void OnEnable()
    {
        // 1. 시작할 때 자식들에 있는 모든 렌더러(Mesh, SkinnedMesh 등)를 싹 긁어모음
        _allRenderers = GetComponentsInChildren<Renderer>();

        _propertyId = Shader.PropertyToID(propertyName);
        _propBlock = new MaterialPropertyBlock();
    }

    void Update()
    {
        // 렌더러가 없으면 리턴
        if (_allRenderers == null || _allRenderers.Length == 0) return;

        // 2. 찾아낸 7개의 렌더러를 반복문으로 돌며 값 적용
        for (int i = 0; i < _allRenderers.Length; i++)
        {
            Renderer r = _allRenderers[i];
            if (r == null) continue;

            // 기존 속성값 유지하면서 블록 가져오기
            r.GetPropertyBlock(_propBlock);

            // 값 변경
            _propBlock.SetFloat(_propertyId, dissolveAmount);

            // 렌더러에 적용
            r.SetPropertyBlock(_propBlock);
        }
    }

    // 에디터에서 컴포넌트 추가하거나 리셋할 때 렌더러 자동 갱신
    void Reset()
    {
        OnEnable();
    }
}