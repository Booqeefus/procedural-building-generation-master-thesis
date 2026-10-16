using UnityEngine;
using System;

[Serializable]
public struct RotateDefinition
{
    
    public enum RotationAxis { X, Y, Z }
    public float angle;
    public RotationAxis axis;
    public Color debugColor; //Color to use for debugging the rotation
    public RotateDefinition( RotationAxis axis, float angle, Color debugColor)
    {
        this.angle = angle;
        this.axis = axis;
        this.debugColor = debugColor;
    }
}