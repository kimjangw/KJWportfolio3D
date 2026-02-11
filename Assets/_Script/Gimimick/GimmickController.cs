using System.Collections.Generic;
using UnityEngine;

public class GimmickController : MonoBehaviour
{
    public enum ColliderControlMode
    {
        ToggleIsTrigger,
        ToggleEnabled
    }

    [Header("Control Mode")]
    public ColliderControlMode controlMode = ColliderControlMode.ToggleIsTrigger;

    [Header("Layer Names")]
    public string phaseALayerName = "PhaseA";
    public string phaseBLayerName = "PhaseB";
    public string desolveLayerName = "Desolve";

    int phaseALayer;
    int phaseBLayer;
    int desolveLayer;

    readonly List<BoxCollider> phaseAColliders = new List<BoxCollider>(256);
    readonly List<BoxCollider> phaseBColliders = new List<BoxCollider>(256);
    readonly List<BoxCollider> desolveColliders = new List<BoxCollider>(256);

    bool isPhaseAActive = true;
    bool isDesolveSolid = true;

    void Awake()
    {
        phaseALayer = LayerMask.NameToLayer(phaseALayerName);
        phaseBLayer = LayerMask.NameToLayer(phaseBLayerName);
        desolveLayer = LayerMask.NameToLayer(desolveLayerName);

        RefreshCache();

        // 시작 상태 반영 1회
        ApplyPhaseRule(isPhaseAActive);
        ApplyDesolveRule(isDesolveSolid);
    }

    public void RefreshCache()
    {
        phaseAColliders.Clear();
        phaseBColliders.Clear();
        desolveColliders.Clear();

        BoxCollider[] childBoxColliders = GetComponentsInChildren<BoxCollider>(true);
        for (int colliderIndex = 0; colliderIndex < childBoxColliders.Length; colliderIndex++)
        {
            BoxCollider eachCollider = childBoxColliders[colliderIndex];
            if (eachCollider == null) continue;

            int colliderLayer = eachCollider.gameObject.layer;

            if (colliderLayer == phaseALayer) phaseAColliders.Add(eachCollider);
            else if (colliderLayer == phaseBLayer) phaseBColliders.Add(eachCollider);
            else if (colliderLayer == desolveLayer) desolveColliders.Add(eachCollider);
        }
    }

    // =========================
    // 외부에서 호출할 API
    // =========================

    public void SetPhaseAActive(bool phaseAActive)
    {
        isPhaseAActive = phaseAActive;
        ApplyPhaseRule(isPhaseAActive);
    }

    public void TogglePhaseAB()
    {
        isPhaseAActive = !isPhaseAActive;
        ApplyPhaseRule(isPhaseAActive);
    }

    public void SetDesolveSolid(bool solid)
    {
        isDesolveSolid = solid;
        ApplyDesolveRule(isDesolveSolid);
    }

    public void ToggleDesolveSolid()
    {
        isDesolveSolid = !isDesolveSolid;
        ApplyDesolveRule(isDesolveSolid);
    }

    // =========================
    // Rules
    // =========================

    void ApplyPhaseRule(bool phaseAActive)
    {
        // A가 실체이면 A는 막힘, B는 통과/꺼짐
        ApplyColliderState(phaseAColliders, isActive: phaseAActive);
        ApplyColliderState(phaseBColliders, isActive: !phaseAActive);
    }

    void ApplyDesolveRule(bool isSolid)
    {
        ApplyColliderState(desolveColliders, isActive: isSolid);
    }

    void ApplyColliderState(List<BoxCollider> targetList, bool isActive)
    {
        for (int colliderIndex = 0; colliderIndex < targetList.Count; colliderIndex++)
        {
            BoxCollider eachCollider = targetList[colliderIndex];
            if (eachCollider == null) continue;

            if (controlMode == ColliderControlMode.ToggleIsTrigger)
            {
                // 실체(Active)=막힘 => Trigger=false
                // 비실체(Inactive)=통과 => Trigger=true
                eachCollider.isTrigger = !isActive;
            }
            else
            {
                // 실체=enabled true / 비실체=enabled false
                eachCollider.enabled = isActive;
            }
        }
    }
}
