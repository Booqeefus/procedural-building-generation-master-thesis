using UnityEngine;

public class ScopeVisualizer : MonoBehaviour
{
    public Scope scope;
    public Color color = Color.yellow;
    public bool drawAsWireframe = true;

     private void Reset()
    {
        scope = new Scope(
            Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one),
            new Vector3(3f, 2f, 4f),
            "default"
        );
        scope.position = transform.position;
    }
    private void OnDrawGizmos()
    {
        if (!enabled) return;

        Gizmos.color = color;

        Matrix4x4 oldMatrix = Gizmos.matrix;
        Gizmos.matrix = scope.matrix;

        if (drawAsWireframe)
            Gizmos.DrawWireCube(Vector3.zero, scope.size);
        else
            Gizmos.DrawCube(Vector3.zero, scope.size);

        Gizmos.matrix = oldMatrix;
    }

}
