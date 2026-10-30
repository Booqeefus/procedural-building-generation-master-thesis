using UnityEngine;
using System.Collections.Generic;

public class DerivationTreeManager:MonoBehaviour
{
    public Material nodeRepresentationMaterial;
    [SerializeField] private Scope rootScope;
    [SerializeField] private Recipe recipe;
    //any good tree needs a root
    private DerivationTreeNode root;
    private DerivationTreeNode backupRoot; //backup root for resetting the tree

    //list of all active nodes in the tree
    private HashSet<DerivationTreeNode> activeNodes = new HashSet<DerivationTreeNode>();
    //maps nodes to their representations in the scene (cubes)
    private Dictionary<DerivationTreeNode, GameObject> nodeCubeMap = new Dictionary<DerivationTreeNode, GameObject>();

    public int recipeStep = 0;
    public int recipeLength = 0;

    private void Awake()
    {
        Initialize();
    }

    private bool Initialize()
    {
        if (root != null)
        {
            return true;
        }

        if (recipe == null)
        {
            Debug.LogError("Cannot initialize the derivation tree without a recipe.", this);
            return false;
        }

        root = new DerivationTreeNode(rootScope);
        CreateCubeRepresentation(root);
        backupRoot = root;
        activeNodes.Add(root);
        recipeStep = 0;
        recipeLength = recipe.GetLength();
        return true;
    }

    private void OnValidate()
{
    if (rootScope.size == Vector3.zero)
    {
        rootScope = Scope.Default;
    }
}


   private void Reset()
    {
        ResetTree();
    }

    public void ResetTree()
    {
        // 1. Loop backwards through children to destroy all physical cube representations attached to this object
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            GameObject child = transform.GetChild(i).gameObject;
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                UnityEditor.Undo.DestroyObjectImmediate(child);
            }
            else
            {
                Destroy(child);
            }
#else
            Destroy(child);
#endif
        }

        // 2. Clear tracking collections and reset state variables
        nodeCubeMap.Clear();
        activeNodes.Clear();
        root = null;
        recipeStep = 0;

        // 3. Re-initialize to respawn the root node
        Initialize();
    }


    
    
    private bool PerformRecipeStep()
    {
        GrammarRule rule = recipe.GetRule(recipeStep);
        if (rule == null)
        {
            Debug.LogError($"No rule found for recipe step {recipeStep}. Cannot perform the step.");
            return false;
        }
        List<DerivationTreeNode> nodesToProcess = new List<DerivationTreeNode>(activeNodes);
        bool found = false;
        foreach (DerivationTreeNode node in nodesToProcess)
        {
    
            if (node.HasTag(rule.targetTag))
            {
                node.ApplyRule(rule);
                activeNodes.Remove(node);
                foreach (DerivationTreeNode child in node.GetChildren())
                {
                    activeNodes.Add(child);
                    CreateCubeRepresentation(child);
                }
                DestroyCubeRepresentation(node);
                found = true;
            }
        }
        if (!found)
        {
            Debug.LogWarning($"No active nodes found with tag '{rule.targetTag}' for recipe step {recipeStep}. Skipping this step.");
        }

        recipeStep++;
        return true;
        
    }

    public void RunWholeRecipe()
    {
        if (!Initialize())
        {
            return;   
        }

        while (recipeStep < recipeLength)
        {
            if (!PerformRecipeStep())
            {
                Debug.LogError("Failed to perform recipe step. Stopping recipe execution.");
                break;
            }
        }
    }

    private void CreateCubeRepresentation(DerivationTreeNode node)
    {
        Scope scope = node.GetScope();
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            UnityEditor.Undo.RegisterCreatedObjectUndo(cube, "Run Whole Recipe");
        }
#endif
        cube.name = string.IsNullOrEmpty(scope.tag) ? "DerivedCube" : scope.tag;

        cube.transform.SetParent(this.transform);
        cube.transform.position = scope.position;
        cube.transform.rotation = scope.rotation;
        cube.transform.localScale = scope.size;

        if (nodeRepresentationMaterial != null)
        {
            Renderer renderer = cube.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = nodeRepresentationMaterial;
            }
        }

        nodeCubeMap[node] = cube;
    }

    private void DestroyCubeRepresentation(DerivationTreeNode node)
    {
        if (nodeCubeMap.TryGetValue(node, out GameObject cube))
        {
            if (cube != null)
            {
#if UNITY_EDITOR
                if (Application.isPlaying)
                {
                    Destroy(cube);
                }
                else
                {
                    UnityEditor.Undo.DestroyObjectImmediate(cube);
                }
#else
                Destroy(cube);
#endif
            }   
            nodeCubeMap.Remove(node);
        }
    }

}

