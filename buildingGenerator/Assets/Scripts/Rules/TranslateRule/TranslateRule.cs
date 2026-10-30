using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(fileName = "TranslateRule", menuName = "Grammar/Rules/Translate Rule")]
public class TranslateRule : GrammarRule
{
    public List<TranslateDefinition> translations;

    public override List<Scope> ApplyRule(Scope parentScope)
    {
        List<Scope> newScopes = new List<Scope>();
        Scope newScope = parentScope;
        foreach (var translation in translations)
        {
            newScope.Translate(translation.translation);
            newScope.debugColor = translation.debugColor;
        }
        newScopes.Add(newScope);
        return newScopes;
    }
}