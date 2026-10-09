using UnityEngine;
using System;

[Serializable]
public struct ExtrudeDefinition
{
    public enum ExtrusionAxis {XPOS,YPOS,ZPOS,XNEG,YNEG,ZNEG}

    public string tag; //Tag to give to new scope
    public ExtrusionAxis axis; //Axis along which to extrude
    public float size;
    public bool isRelative;
    public Color debugColor; //Color for new scope, for debugging purposes
    public ExtrudeDefinition(string tag, ExtrusionAxis axis, float size, bool isRelative, Color debugColor)
    {
        this.tag = tag;
        this.axis = axis;
        this.size = size;
        this.isRelative = isRelative;
        this.debugColor = debugColor;
    }
}