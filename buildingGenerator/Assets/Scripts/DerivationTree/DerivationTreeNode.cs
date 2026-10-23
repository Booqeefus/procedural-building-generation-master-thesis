using UnityEngine;
using System.Collections.Generic;

public class DerivationTreeNode
{
    private bool isActive;
    private Scope scope;
    private List<DerivationTreeNode> children = new List<DerivationTreeNode>();

    public DerivationTreeNode(Scope scope, bool isActive = true)
    {
        this.scope = scope;
        this.isActive = isActive;
    }

    public string GetScopeTag()
    {
        return scope.tag;
    }
    public Scope GetScope()
    {
        return scope;
    }

    public void ApplyRule(GrammarRule rule)
    {
        if (!isActive || rule == null)
        {
            return;
        }

        List<Scope> newScopes = rule.ApplyRule(scope);
        foreach (Scope newScope in newScopes)
        {
            DerivationTreeNode childNode = new DerivationTreeNode(newScope);
            children.Add(childNode);
        }
        isActive = false;
    }

    public bool HasTag(string tag)
    {
        return scope.tag == tag;
    }
    public List<DerivationTreeNode> GetChildren()
    {
        return children;
    }
    public void ClearChildren()
    {
        children.Clear();
    }

}
