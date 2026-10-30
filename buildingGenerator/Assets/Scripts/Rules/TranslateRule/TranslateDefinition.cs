using UnityEngine;
using System;
[Serializable]

public class TranslateDefinition
{
    public Vector3 translation;
    public Color debugColor;
    public TranslateDefinition(Vector3 translation, Color debugColor)
    {
        this.translation = translation;
        this.debugColor = debugColor;
    }
}