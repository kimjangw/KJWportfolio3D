using System.Collections;
using UnityEngine;

public class DesolveGimmick : MonoBehaviour
{
    [Header("Target Material (Dissolve ShaderGraph Material)")]
    public Material targetMaterial;

    [Header("Shader Property Reference")]
    public string desolvePropertyReference = "_DesolveValue"; // ShaderGraph Reference와 동일

    [Header("Control (toggle)")]
    [Range(0f, 1f)]
    public float desolveValue = 1f;

    int desolvePropertyId;
    Coroutine toggleCoroutine;

    void Awake()
    {
        desolvePropertyId = Shader.PropertyToID(desolvePropertyReference);
        Apply();
    }

    void Apply()
    {
        if (targetMaterial == null)
        {
            return;
        }

        if (!targetMaterial.HasProperty(desolvePropertyId))
        {
            Debug.LogWarning(
                $"[DesolveGimmick] Material '{targetMaterial.name}' does not have property '{desolvePropertyReference}'.",
                this
            );
            return;
        }

        targetMaterial.SetFloat(desolvePropertyId, desolveValue);
    }

    // 클릭 시 호출: 0 ↔ 1 토글 + durationSeconds 동안 보간
    public void DesolveToggle(float durationSeconds)
    {
        float targetValue = (desolveValue >= 0.5f) ? 0f : 1f;

        if (durationSeconds <= 0f)
        {
            desolveValue = targetValue;
            Apply();
            return;
        }

        if (toggleCoroutine != null)
        {
            StopCoroutine(toggleCoroutine);
        }

        toggleCoroutine = StartCoroutine(DesolveCoroutine(targetValue, durationSeconds));
    }

    IEnumerator DesolveCoroutine(float targetValue, float durationSeconds)
    {
        float startValue = desolveValue;
        float elapsedSeconds = 0f;

        while (elapsedSeconds < durationSeconds)
        {
            elapsedSeconds += Time.deltaTime;

            float normalizedTime = elapsedSeconds / durationSeconds;
            desolveValue = Mathf.Lerp(startValue, targetValue, normalizedTime);

            Apply();
            yield return null;
        }

        desolveValue = targetValue;
        Apply();

        toggleCoroutine = null;
    }
}
