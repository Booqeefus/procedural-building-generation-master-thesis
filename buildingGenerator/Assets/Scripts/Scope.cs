using UnityEngine;
using System;

[Serializable]
public struct Scope
{
    public Matrix4x4 matrix;
    public Vector3 size;
    public string tag;
    public Scope(Matrix4x4 matrix, Vector3 size, string tag)
    {
        this.matrix = matrix;
        this.size = size;
        this.tag = tag;
    }
    
}
