using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Delete Rule", menuName = "Grammar/Rules/Delete Rule")]
public class DeleteRule : GrammarRule
{
    public override List<Scope> ApplyRule(Scope parentScope)
    {
        return new List<Scope>();
    }
}