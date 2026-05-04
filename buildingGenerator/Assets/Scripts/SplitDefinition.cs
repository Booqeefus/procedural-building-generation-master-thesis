using UnityEngine;
using System;

[Serializable]
public struct SplitDefinition
{
    public string tag; //Tag to give to new scope
    public float size; //Size of the new scope
    public bool isRelative;
    public Color debugColor; //Color to use for debugging the split
    public SplitDefinition(string tag, float size, bool isRelative, Color debugColor)
    {
        this.tag = tag;
        this.size = size;
        this.isRelative = isRelative;
        this.debugColor = debugColor;
    }

    
}