using UnityEngine;
using System.Collections.Generic;
public class Recipe
{
    private List<GrammarRule> rules;
    public Recipe(List<GrammarRule> rules)
    {
        this.rules = rules;
    }

    public GrammarRule GetRule(int index)
    {
        if (index < 0 || index >= rules.Count)
        {
            Debug.LogError($"Index {index} is out of bounds for recipe with {rules.Count} rules.");
            return null;
        }
        return rules[index];
    }
    public int GetLength()
    {
        return rules.Count;
    }
    
}
