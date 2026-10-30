using UnityEngine;
using System;

[Serializable]
public struct Scope
{
    public Matrix4x4 matrix;
    public Vector3 size;
    public string tag;
    public Color debugColor;

    public Scope(Matrix4x4 matrix, Vector3 size, string tag, Color debugColor = default)
    {
        this.matrix = Matrix4x4.TRS(
            matrix.GetColumn(3),
            Quaternion.LookRotation(matrix.GetColumn(2), matrix.GetColumn(1)),
            Vector3.one
        );
        this.size = size;
        this.tag = string.IsNullOrEmpty(tag) ? "default" : tag;
        this.debugColor = debugColor == default ? Color.white : debugColor;
    }

    public Vector3 position
    {
        get => matrix.MultiplyPoint3x4(Vector3.zero);
        set
        {
            Quaternion rotation = this.rotation;
            matrix = Matrix4x4.TRS(value, rotation, Vector3.one);
        }
    }

    public Quaternion rotation
    {
        get => Quaternion.LookRotation(matrix.GetColumn(2), matrix.GetColumn(1));
        set
        {
            Vector3 pos = this.position;
            matrix = Matrix4x4.TRS(pos, value, Vector3.one);
        }
    }

    public Vector3 scale
    {
        get => size;
        set
        {
            size = value;
        }
    }

    public void SetTRS(Vector3 position, Quaternion rotation, Vector3 scale)
    {
        matrix = Matrix4x4.TRS(position, rotation, Vector3.one);
        size = scale;
    }

    public void Translate(Vector3 offset)
    {
        position += offset;
    }

    public void Rotate(Quaternion rotation)
    {
        this.rotation *= rotation;
    }
    public void Scale(Vector3 scaleFactors)
    {
        size = Vector3.Scale(size, scaleFactors);
    }

    // Static helper to get a fully initialized default scope
    public static Scope Default
    {
        get
        {
            return new Scope(
                Matrix4x4.identity,
                Vector3.one,
                "default",
                Color.white
            );
        }
    }
    
}
