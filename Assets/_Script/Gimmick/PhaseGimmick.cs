using System.Collections;
using UnityEngine;

public class PhaseGimmick : MonoBehaviour
{
    [Header("Target Materials")]
    public Material shaderGraphMaterialA;
    public Material shaderGraphMaterialB;

    [Header("Shader Property Reference")]
    public string phasePropertyReference = "PhaseValue";

    [Header("Control (toggle)")]
    [Range(-0.6f, 0.6f)]
    public float phaseValue = 0.6f;

    int phasePropertyId;
    Coroutine toggleCoroutine;

    void Awake()
    {
        phasePropertyId = Shader.PropertyToID(phasePropertyReference);
        Apply();
    }

    void Apply()
    {
        ApplyToMaterial(shaderGraphMaterialA);
        ApplyToMaterial(shaderGraphMaterialB);
    }

    void ApplyToMaterial(Material targetMaterial)
    {
        if (targetMaterial == null)
        {
            return;
        }

        if (!targetMaterial.HasProperty(phasePropertyId))
        {
            Debug.LogWarning(
                $"[PhaseGimmick] Material '{targetMaterial.name}' does not have property '{phasePropertyReference}'.",
                this
            );
            return;
        }

        targetMaterial.SetFloat(phasePropertyId, phaseValue);
    }

    // 클릭용: 호출할 함수 1개만 남김
    // 현재 값 기준으로 0 <-> 1 토글하면서 durationSeconds 동안 보간
    public void PhaseToggle(float durationSeconds)
    {
        float targetValue = (phaseValue > 0f) ? -0.6f : 0.6f;

        if (durationSeconds <= 0f)
        {
            phaseValue = targetValue;
            Apply();
            return;
        }

        if (toggleCoroutine != null)
        {
            StopCoroutine(toggleCoroutine);
        }

        toggleCoroutine = StartCoroutine(PhaseCoroutine(targetValue, durationSeconds));
    }

    IEnumerator PhaseCoroutine(float targetValue, float durationSeconds)
    {
        float startValue = phaseValue;
        float elapsedSeconds = 0f;

        while (elapsedSeconds < durationSeconds)
        {
            elapsedSeconds += Time.deltaTime;

            float normalizedTime = elapsedSeconds / durationSeconds;
            phaseValue = Mathf.Lerp(startValue, targetValue, normalizedTime);

            Apply();
            yield return null;
        }

        phaseValue = targetValue;
        Apply();

        toggleCoroutine = null;
    }
}
