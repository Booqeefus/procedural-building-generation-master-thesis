using UnityEngine;
using System.Collections.Generic;

public abstract class GrammarRule : ScriptableObject
{
    public string targetTag;
    abstract public List<Scope> ApplyRule(Scope parentScope);
}