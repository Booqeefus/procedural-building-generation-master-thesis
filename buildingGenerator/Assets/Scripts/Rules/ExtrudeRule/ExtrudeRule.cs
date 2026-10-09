using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ExtrudeRule", menuName = "Grammar/Rules/Extrude Rule")]
public class ExtrudeRule : GrammarRule
{
    public List<ExtrudeDefinition> extrusions;
    public override List<Scope> ApplyRule(Scope parentScope)
    {
        List<Scope> newScopes = new List<Scope>();
        
        foreach (ExtrudeDefinition extrusion in extrusions)
        {
            int axis;
            float direction;

            switch (extrusion.axis)
            {
                case ExtrudeDefinition.ExtrusionAxis.XPOS:
                    axis = 0;
                    direction = 1f;
                    break;
                case ExtrudeDefinition.ExtrusionAxis.XNEG:
                    axis = 0;
                    direction = -1f;
                    break;
                case ExtrudeDefinition.ExtrusionAxis.YPOS:
                    axis = 1;
                    direction = 1f;
                    break;
                case ExtrudeDefinition.ExtrusionAxis.YNEG:
                    axis = 1;
                    direction = -1f;
                    break;
                case ExtrudeDefinition.ExtrusionAxis.ZPOS:
                    axis = 2;
                    direction = 1f;
                    break;
                case ExtrudeDefinition.ExtrusionAxis.ZNEG:
                    axis = 2;
                    direction = -1f;
                    break;
                default:
                    continue;
            }

            Vector3 newSize = parentScope.size;
            float originalAxisSize = GetAxisSize(newSize, axis);
            float extrudedAxisSize = extrusion.isRelative
                ? originalAxisSize * extrusion.size
                : originalAxisSize + extrusion.size;
            SetAxisSize(ref newSize, axis, extrudedAxisSize);

            Vector3 localOffset = Vector3.zero;
            SetAxisSize(ref localOffset, axis, direction * (extrudedAxisSize - originalAxisSize) / 2f);

            Matrix4x4 childMatrix = parentScope.matrix * Matrix4x4.Translate(localOffset);
            string newTag = string.IsNullOrEmpty(extrusion.tag) ? parentScope.tag : extrusion.tag;
            newScopes.Add(new Scope(childMatrix, newSize, newTag, extrusion.debugColor));
        }

        return newScopes;
    }

    private float GetAxisSize(Vector3 value, int axis)
    {
        return axis == 0 ? value.x : axis == 1 ? value.y : value.z;
    }

    private void SetAxisSize(ref Vector3 value, int axis, float size)
    {
        if (axis == 0) value.x = size;
        else if (axis == 1) value.y = size;
        else value.z = size;
    }
}