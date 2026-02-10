using System.Collections;
using UnityEngine;

public class DesolveGimmick : MonoBehaviour
{
    [Header("Target Material (Dissolve ShaderGraph Material)")]
    public Material targetMaterial;

    [Header("Shader Property Reference")]
    public string desolvePropertyReference = "_DesolveValue"; // 스크린샷 Reference와 동일해야 함

    [Header("Control")]
    [Range(0f, 1f)]
    public float desolveValue = 1f;

    [Header("Range")]
    public float minDesolveValue = 0f;
    public float maxDesolveValue = 1f;

    int desolvePropertyId;
    Coroutine desolveSmoothCoroutine;

    void Awake()
    {
        desolvePropertyId = Shader.PropertyToID(desolvePropertyReference);
        Apply();
    }

    void OnValidate()
    {
        desolvePropertyId = Shader.PropertyToID(desolvePropertyReference);
        Apply();
    }

    public void Apply()
    {
        if (targetMaterial == null)
        {
            return;
        }

        if (!targetMaterial.HasProperty(desolvePropertyId))
        {
            Debug.LogWarning(
                $"[DesolveGimmick] Material '{targetMaterial.name}' does not have property '{desolvePropertyReference}'. " +
                $"Check ShaderGraph property Reference name.",
                this
            );
            return;
        }

        float clampedValue = Mathf.Clamp(desolveValue, minDesolveValue, maxDesolveValue);
        targetMaterial.SetFloat(desolvePropertyId, clampedValue);
    }

    // =========================
    // Smooth control
    // =========================

    public void SetDesolveImmediate(float targetValue)
    {
        desolveValue = Mathf.Clamp(targetValue, minDesolveValue, maxDesolveValue);
        Apply();
    }

    public void SetDesolveSmooth(float targetValue, float durationSeconds)
    {
        float clampedTargetValue = Mathf.Clamp(targetValue, minDesolveValue, maxDesolveValue);

        if (durationSeconds <= 0f)
        {
            SetDesolveImmediate(clampedTargetValue);
            return;
        }

        if (desolveSmoothCoroutine != null)
        {
            StopCoroutine(desolveSmoothCoroutine);
        }

        desolveSmoothCoroutine = StartCoroutine(CoSetDesolveSmooth(clampedTargetValue, durationSeconds));
    }

    // 클릭할 때마다 0 ↔ 1 스왑(보간)
    public void ToggleDesolveSmooth(float durationSeconds)
    {
        float midValue = (minDesolveValue + maxDesolveValue) * 0.5f;
        float targetValue = (desolveValue >= midValue) ? minDesolveValue : maxDesolveValue;
        SetDesolveSmooth(targetValue, durationSeconds);
    }

    IEnumerator CoSetDesolveSmooth(float targetValue, float durationSeconds)
    {
        float startValue = desolveValue;
        float elapsedSeconds = 0f;

        while (elapsedSeconds < durationSeconds)
        {
            elapsedSeconds += Time.deltaTime;

            float normalizedTime = elapsedSeconds / durationSeconds;
            float interpolatedValue = Mathf.Lerp(startValue, targetValue, normalizedTime);

            desolveValue = Mathf.Clamp(interpolatedValue, minDesolveValue, maxDesolveValue);
            Apply();

            yield return null;
        }

        desolveValue = Mathf.Clamp(targetValue, minDesolveValue, maxDesolveValue);
        Apply();

        desolveSmoothCoroutine = null;
    }
}
