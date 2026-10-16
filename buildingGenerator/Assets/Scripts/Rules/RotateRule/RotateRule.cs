using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "RotateRule", menuName = "Grammar/Rules/Rotate Rule")]

public class RotateRule : GrammarRule
{
    public List<RotateDefinition> rotations;

    public override List<Scope> ApplyRule(Scope parentScope)
    {
        List<Scope> newScopes = new List<Scope>();

        Scope scope = parentScope;
        foreach (var rotation in rotations)
        {
            Quaternion rotationQuat = Quaternion.identity;
            switch (rotation.axis)
            {
                case RotateDefinition.RotationAxis.X:
                    rotationQuat = Quaternion.Euler(rotation.angle, 0f, 0f);
                    break;
                case RotateDefinition.RotationAxis.Y:
                    rotationQuat = Quaternion.Euler(0f, rotation.angle, 0f);
                    break;
                case RotateDefinition.RotationAxis.Z:
                    rotationQuat = Quaternion.Euler(0f, 0f, rotation.angle);
                    break;
            }
            scope.Rotate(rotationQuat);
        }
        scope.tag = parentScope.tag; // Keep the original tag for the final scope
        newScopes.Add(scope);
        return newScopes;
    }
    
}