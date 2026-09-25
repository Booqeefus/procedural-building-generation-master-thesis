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

        // 1. Calculate total fixed vs. relative space
        float parentSizeOnAxis = GetAxisSize(parentScope.size, splitAxis);
        float fixedTotal = 0f;
        float relativeWeightSum = 0f;

        foreach (var split in splits)
        {
            if (split.isRelative)
                relativeWeightSum += split.size;
            else
                fixedTotal += split.size;
        }

        // 2. Determine how much space is left for relative segments
        float remainingSpace = Mathf.Max(0f, parentSizeOnAxis - fixedTotal);
        float cursor = -parentSizeOnAxis / 2f; // Start at the parent-local negative edge (bottom/left/front)

        // 3. Create the sub-scopes
        foreach (var split in splits)
        {
            float sizeOnAxis = split.isRelative
                ? (split.size / (relativeWeightSum > 0f ? relativeWeightSum : 1f)) * remainingSpace
                : split.size;

            Vector3 newSize = parentScope.size;
            SetAxisSize(ref newSize, splitAxis, sizeOnAxis);

            Vector3 localOffset = Vector3.zero;
            SetAxisSize(ref localOffset, splitAxis, cursor + sizeOnAxis / 2f); // Center the child scope in the parent space

            Matrix4x4 childMatrix = parentScope.matrix * Matrix4x4.Translate(localOffset);
            Scope newScope = new Scope(childMatrix, newSize, split.tag, split.debugColor);
            newScopes.Add(newScope);

            cursor += sizeOnAxis; // Move the cursor along the parent-local axis
        }

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