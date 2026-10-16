using UnityEngine;
using System;

[Serializable]
public struct ScaleDefinition
{
    public Vector3 scaleFactors; //Scale factor to apply to the new scope
    public Color debugColor; //Color to use for debugging the scale
    public ScaleDefinition( Vector3 scaleFactors, Color debugColor)
    {
        this.scaleFactors = scaleFactors;
        this.debugColor = debugColor;
    }
}