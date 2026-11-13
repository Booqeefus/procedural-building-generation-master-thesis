using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DerivationTreeManager))]
public class DerivationTreeManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Run Whole Recipe"))
        {
            ((DerivationTreeManager)target).RunWholeRecipe();
        }
        if(GUILayout.Button("Perform Recipe Step"))
        {
            ((DerivationTreeManager)target).PerformRecipeStep();
        }
    }
}
