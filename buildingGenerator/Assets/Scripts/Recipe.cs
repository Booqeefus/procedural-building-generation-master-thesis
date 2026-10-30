using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Recipe", menuName = "Grammar/Recipes")]
public class Recipe : ScriptableObject
{
    public List<GrammarRule> rules;
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
