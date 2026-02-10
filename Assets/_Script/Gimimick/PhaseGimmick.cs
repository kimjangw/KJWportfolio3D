using System.Collections;
using UnityEngine;

public class PhaseGimmick : MonoBehaviour
{
    [Header("Target Materials")]
    public Material shaderGraphMaterialA;
    public Material shaderGraphMaterialB;

    [Header("Shader Property Reference")]
    public string phasePropertyReference = "PhaseValue";

    [Header("Control")]
    [Range(-0.6f, 0.6f)]
    public float phaseValue = 0.6f;

    [Header("Phase Range")]
    public float minPhaseValue = -0.6f;
    public float maxPhaseValue = 0.6f;

    int phasePropertyId;
    Coroutine phaseSmoothCoroutine;

    void Awake()
    {
        phasePropertyId = Shader.PropertyToID(phasePropertyReference);
        Apply();
    }

    void OnValidate()
    {
        phasePropertyId = Shader.PropertyToID(phasePropertyReference);
        Apply();
    }

    public void Apply()
    {
        ApplyPhaseValueToMaterial(shaderGraphMaterialA);
        ApplyPhaseValueToMaterial(shaderGraphMaterialB);
    }

    void ApplyPhaseValueToMaterial(Material targetMaterial)
    {
        if (targetMaterial == null)
        {
            return;
        }

        if (!targetMaterial.HasProperty(phasePropertyId))
        {
            Debug.LogWarning(
                $"[Gimmick] Material '{targetMaterial.name}' does not have property '{phasePropertyReference}'. " +
                $"Check ShaderGraph property Reference name.",
                this
            );
            return;
        }

        float clampedPhaseValue = Mathf.Clamp(phaseValue, minPhaseValue, maxPhaseValue);
        targetMaterial.SetFloat(phasePropertyId, clampedPhaseValue);
    }

    // =========================
    // Smooth control
    // =========================

    public void SetPhaseImmediate(float targetPhaseValue)
    {
        phaseValue = Mathf.Clamp(targetPhaseValue, minPhaseValue, maxPhaseValue);
        Apply();
    }

    public void SetPhaseSmooth(float targetPhaseValue, float durationSeconds)
    {
        float clampedTargetPhaseValue = Mathf.Clamp(targetPhaseValue, minPhaseValue, maxPhaseValue);

        if (durationSeconds <= 0f)
        {
            SetPhaseImmediate(clampedTargetPhaseValue);
            return;
        }

        if (phaseSmoothCoroutine != null)
        {
            StopCoroutine(phaseSmoothCoroutine);
        }

        phaseSmoothCoroutine = StartCoroutine(CoSetPhaseSmooth(clampedTargetPhaseValue, durationSeconds));
    }

    public void TogglePhaseSmooth(float durationSeconds)
    {
        float midPhaseValue = (minPhaseValue + maxPhaseValue) * 0.5f;
        float targetPhaseValue = (phaseValue >= midPhaseValue) ? minPhaseValue : maxPhaseValue;
        SetPhaseSmooth(targetPhaseValue, durationSeconds);
    }

    IEnumerator CoSetPhaseSmooth(float targetPhaseValue, float durationSeconds)
    {
        float startPhaseValue = phaseValue;
        float elapsedSeconds = 0f;

        while (elapsedSeconds < durationSeconds)
        {
            elapsedSeconds += Time.deltaTime;

            float normalizedTime = elapsedSeconds / durationSeconds;
            float interpolatedPhaseValue = Mathf.Lerp(startPhaseValue, targetPhaseValue, normalizedTime);

            phaseValue = Mathf.Clamp(interpolatedPhaseValue, minPhaseValue, maxPhaseValue);
            Apply();

            yield return null;
        }

        phaseValue = Mathf.Clamp(targetPhaseValue, minPhaseValue, maxPhaseValue);
        Apply();

        phaseSmoothCoroutine = null;
    }
}
