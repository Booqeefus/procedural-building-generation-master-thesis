using UnityEngine;
using System.Collections.Generic;

public class ScopeVisualizer : MonoBehaviour
{
    public Scope scope;
    public GrammarRule ruleToApply;
    public Color color = Color.yellow;
    public bool drawAsWireframe = true;

    private void Reset()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Transform child = transform.GetChild(i);
#if UNITY_EDITOR
            UnityEditor.EditorApplication.delayCall += () => DestroyImmediate(child.gameObject);
#else
            Destroy(child.gameObject);
#endif
        }

        scope = new Scope(
            Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one),
            new Vector3(1f, 1f, 1f),
            "default",
            color
        );
        scope.position = transform.position;
        enabled = true;
    }

    private void OnDrawGizmos()
    {
        if (!enabled) return;

        Gizmos.color = color;

        Matrix4x4 oldMatrix = Gizmos.matrix;
        Gizmos.matrix = scope.matrix;

        if (drawAsWireframe)
            Gizmos.DrawWireCube(Vector3.zero, scope.size);
        else
            Gizmos.DrawCube(Vector3.zero, scope.size);

        Gizmos.matrix = oldMatrix;
    }

    [ContextMenu("Apply Rule to Own Scope")]
    public void ApplyRuleToOwnScopeFromInspector()
    {
        if (ruleToApply != null)
            ApplyRuleToOwnScope(ruleToApply);
    }

    public void ApplyRuleToOwnScope(GrammarRule rule)
    {
        if (rule == null)
            return;

        List<Scope> newScopes = rule.ApplyRule(scope);

        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Transform child = transform.GetChild(i);
            if (child.GetComponent<ScopeVisualizer>() != null)
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.delayCall += () => DestroyImmediate(child.gameObject);
#else
                Destroy(child.gameObject);
#endif
            }
        }

        foreach (Scope childScope in newScopes)
        {
            GameObject childGo = new GameObject(string.IsNullOrEmpty(childScope.tag) ? "Scope" : childScope.tag);
            childGo.transform.SetParent(transform, true);
            childGo.transform.position = childScope.position;
            childGo.transform.rotation = childScope.rotation;
            childGo.transform.localScale = childScope.scale;

            ScopeVisualizer visualizer = childGo.AddComponent<ScopeVisualizer>();
            visualizer.scope = childScope;
            visualizer.color = childScope.debugColor;
            visualizer.drawAsWireframe = drawAsWireframe;
        }

        enabled = false;
    }
}
