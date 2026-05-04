using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "SplitRule", menuName = "Grammar/Rules/Split Rule")]
public class SplitRule : GrammarRule
{
    public enum Axis { X, Y, Z }
    public Axis splitAxis = Axis.X; // Choose which local direction to slice
    public List<SplitDefinition> splits;

    public override List<Scope> ApplyRule(Scope parentScope)
    {
        List<Scope> newScopes = new List<Scope>();
        
        // 1. Calculate Total Fixed vs. Relative space
        float parentSizeOnAxis = GetAxisSize(parentScope.size, splitAxis);
        float fixedTotal = 0;
        float relativeWeightSum = 0;

        foreach (var split in splits)
        {
            if (split.isRelative) relativeWeightSum += split.size;
            else fixedTotal += split.size;
        }

        // 2. Determine how much space is left for relative segments
        float remainingSpace = Mathf.Max(0, parentSizeOnAxis - fixedTotal);
        float cursor = 0; // Tracks the local starting position of the next segment

        // 3. Create the sub-scopes
    
        return newScopes;
    }

    private float GetAxisSize(Vector3 v, Axis a) => a == Axis.X ? v.x : a == Axis.Y ? v.y : v.z;
    
    private void SetAxisSize(ref Vector3 v, Axis a, float val)
    {
        if (a == Axis.X) v.x = val;
        else if (a == Axis.Y) v.y = val;
        else v.z = val;
    }
}