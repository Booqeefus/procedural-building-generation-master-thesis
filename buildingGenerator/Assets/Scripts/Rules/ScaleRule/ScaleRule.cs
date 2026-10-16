using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(fileName = "New Scale Rule", menuName = "Grammar/Rules/Scale Rule")]

public class ScaleRule : GrammarRule
{
    //No idea why someone would not just do the scaling at once, but phuck it
    public List<ScaleDefinition> scales;

    public override List<Scope> ApplyRule(Scope parentScope)
    {
        List<Scope> newScopes = new List<Scope>();

        Scope scope = parentScope;
        foreach (var scale in scales)
        {
            scope.Scale(scale.scaleFactors);
        }
        newScopes.Add(scope);
        return newScopes;
    }
}