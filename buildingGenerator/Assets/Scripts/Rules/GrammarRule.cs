using UnityEngine;
using System.Collections.Generic;

public abstract class GrammarRule : ScriptableObject
{
        abstract public List<Scope> ApplyRule(Scope parentScope);
}