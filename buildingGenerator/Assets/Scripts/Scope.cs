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

    public Vector3 position
    {
        get => matrix.MultiplyPoint3x4(Vector3.zero);
        set
        {
            Vector3 scale = this.scale;
            Quaternion rotation = this.rotation;
            matrix = Matrix4x4.TRS(value, rotation, scale);
        }
    }

    public Quaternion rotation
    {
        get
        {
            Vector3 scale = this.scale;
            return Quaternion.LookRotation(
                matrix.GetColumn(2),
                matrix.GetColumn(1)
            );
        }
        set
        {
            Vector3 pos = this.position;
            Vector3 scale = this.scale;
            matrix = Matrix4x4.TRS(pos, value, scale);
        }
    }

    public Vector3 scale
    {
        get
        {
            Vector3 scale;
            scale.x = matrix.GetColumn(0).magnitude;
            scale.y = matrix.GetColumn(1).magnitude;
            scale.z = matrix.GetColumn(2).magnitude;
            return scale;
        }
        set
        {
            Vector3 pos = this.position;
            Quaternion rot = this.rotation;
            matrix = Matrix4x4.TRS(pos, rot, value);
        }
    }

    public void SetTRS(Vector3 position, Quaternion rotation, Vector3 scale)
    {
        matrix = Matrix4x4.TRS(position, rotation, scale);
    }

    public void Translate(Vector3 offset)
    {
        position += offset;
    }
    
}
